using System.Diagnostics;
using UnityEngine;

// 앞으로 만들 모든 발사체들은 이 클래스를 상속 받아야 한다. 
public abstract class ProjectileBase : MonoBehaviour
{
    protected MovementRigidbody2D movementRigidbody2D;
    public virtual void Setup(SkillBase skillBase, float damage) { } // 광역 스킬 전용 Setup 상속받는 클래스에서 정의 (광역 스킬을 정의하는 클래스에서 정의 하면 됨)
    public virtual void Setup(EntityBase target, float damage, int maxCount, int index)
    {
        Setup(target, damage);
    }

    public virtual void Setup(EntityBase target, float damage) // 기본 세팅은 오버라이딩을 할 수도 있고 안해도 됨
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();

        // 발사체 크기를 20%에서 100%로 확대
        GetComponent<ScaleEffect>().Play(transform.localScale * 0.2f, transform.localScale);

        // 적 오브젝트와 충돌 처리
        GetComponent<ProjectileCollision2D>().Setup(target, damage);
    }

    private void Update()
    {
        Process();  // 프로세스 메소드는 프레임마다 호출 되도록 업데이트 메서드에 배치 
    }

    public abstract void Process(); // 이 클래스를 상속 받는 클래스들에서 수현하게 abstract 수식 (추상 메서드)
}
