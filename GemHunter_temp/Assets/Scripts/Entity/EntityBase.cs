using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField]
    protected EntityStats stats; // 이 클래스를 상속받는 친구들은 필드를 바로 접근하게 하기 위해서 protected

    public EntityStats Stats => stats; // 읽기 전용 프로퍼티 
    public bool IsDead => stats.currentHp <= 0; //읽기 전용 체력이 0 이하면 true 반환 (죽음 여부 판단용)
    protected virtual void Setup() // 가상 메소드(virtual)로 선언 이걸 상속 받는 친구들이 오버라이드 하게 하기 위해서 
    {
        stats.currentHp = stats.MaxHp; // 최대 체력으로 초기화 게임 시작 할 때 
    }

    public void TakeDamaget(float damage)
    {
        if (IsDead) return; // 죽었으면 반환

        stats.currentHp = stats.currentHp - damage > 0 ?
            stats.currentHp - damage : 0;

        if(stats.currentHp == 0)
        {
            // 사망 처리 로직 
        }
    }
}
