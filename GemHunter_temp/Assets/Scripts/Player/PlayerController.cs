using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementRigidbody2D movement2D;
    private PlayerRenderer playerRenderer;
    private PlayerBase playerBase;
    private Vector2 moveInput = Vector2.zero;

    private void Awake()
    {
        movement2D = GetComponent<MovementRigidbody2D>();
        playerRenderer = GetComponentInChildren<PlayerRenderer>();
        playerBase = GetComponent<PlayerBase>();
    }

    private void Update()
    {
        // 플레이어 이동 여부 검사 
        playerBase.IsMoved = moveInput.x != 0 || moveInput.y != 0; // 지역 변수에서 바꾼이유는 여러스크립트에서 이동중을 체크해야 하기 위해서이다,
                                                                   // Update() 함수 내부의 변수는 다른 곳에서 접근이 불가능하기 때문
        // 플레이어 좌우 반전
        if (moveInput.x != 0) playerRenderer.SpriteFlipX(moveInput.x);
        // 플레이어 애니메이션 재생
        playerRenderer.OnMovement(playerBase.IsMoved ? 1 : 0);
        // 먼지 효과 재생
        playerRenderer.OnFootStepEffect(playerBase.IsMoved);
        // 플레이어 이동
        movement2D.MoveTo(moveInput);

        // 목표 방향으로 플레이어 무기 회전 
        playerRenderer.LookRotation(playerBase);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed || context.canceled)
        {   
            moveInput = context.ReadValue<Vector2>();
        }
    }
}
