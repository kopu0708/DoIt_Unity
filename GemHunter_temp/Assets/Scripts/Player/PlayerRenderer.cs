using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform playerModel; // 좌우 반전을 위한 Transform
    [SerializeField]
    private ParticleSystem footStepEffect;
    private ParticleSystem.EmissionModule footEmission;
    private Animator animator;

    private void Awake()
    {
        footEmission = footStepEffect.emission;
        animator = GetComponent<Animator>();
    }
    
    public void OnMovement(float speed)
    {
        animator.SetFloat("moveSpeed", speed);
    }

    public void OnFootStepEffect(bool isMoved)
    {
        footEmission.rateOverTime = isMoved == true ? 20 : 0;
    }

    // SpriteRenderer 컴포넌트의 Flip을 이용해 이미지를 반전하면 
    // 화면에 출력하는 이미지 자체만 반전하므로 
    // 플레이어의 전방 특정 위치에서 발사체를 생성할 때처럼 
    // 방향을 전환해야 할 때는 Transform.Scale.x를 -1, 1과 같이 설정
    public void SpriteFlipX(float x)
    {
        Vector3 currentScale = playerModel.localScale;
        currentScale.x = x < 0 ? -1.5f : 1.5f;
        playerModel.localScale = currentScale;
    }
}
