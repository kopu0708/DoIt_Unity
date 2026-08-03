using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform playerModel; // 좌우 반전을 위한 Transform
    [SerializeField]
    private Transform playerArmsModel;
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

    public void LookRotation(PlayerBase playerBase)
    {
        if(playerBase.IsMoved == true)
        {
            playerArmsModel.rotation = Quaternion.identity;
        }

        else
        {
            if (playerBase.Target == null) return;

            Vector3 target = playerBase.Target.MiddlePoint;
            //목표물이 플레이어 왼쪽이라면 -1, 오른쪽이면 1
            float flip = target.x - transform.position.x < 0 ? -1 : 1;
            // 플레이어 좌우 반전
            SpriteFlipX(flip);
            // 플레이어 무기 회전
            // 왼쪽을 볼 때는 부모 오브젝에 의해 회전이 적용되어 무기 방향이 틀어지도록 180만큼 가중치를 줌 
            playerArmsModel.rotation = Utils.RotateToTarget(
                playerArmsModel.position, target, (1 - flip) * 90);
        }
    }
}
