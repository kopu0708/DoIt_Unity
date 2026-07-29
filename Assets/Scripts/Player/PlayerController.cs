using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementRigidbody2D movement2D;
    private PlayerRenderer playerRenderer;
    private Vector2 moveInput = Vector2.zero;

    private void Awake()
    {
        movement2D = GetComponent<MovementRigidbody2D>();
        playerRenderer = GetComponentInChildren<PlayerRenderer>();
    }

    private void Update()
    {
        // 플레이어 이동 여부 검사 
        bool isMoved = moveInput.x != 0 || moveInput.y != 0;
        // 플레이어 좌우 반전
        if (moveInput.x != 0) playerRenderer.SpriteFlipX(moveInput.x);
        // 플레이어 애니메이션 재생
        playerRenderer.OnMovement(isMoved ? 1 : 0);
        // 먼지 효과 재생
        playerRenderer.OnFootStepEffect(isMoved);
        // 플레이어 이동
        movement2D.MoveTo(moveInput);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed || context.canceled)
        {   
            moveInput = context.ReadValue<Vector2>();
        }
    }
}
