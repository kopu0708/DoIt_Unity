using System;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField]
    private EntityStats stats; // 이 클래스를 상속받는 친구들은 필드를 바로 접근하게 하기 위해서 protected
    [SerializeField]
    private Transform middlePoint; // Player, Enemy 오브젝트의 위치는 화면에 출력되는 캐릭터 이미지의 발 위치로 설정된다.
                                   // 몸통과 같은 오브젝트의 중심 위치를 기준으로 공격할 수 있도록 별도의 빈 오브젝트를 만들어 각 오브젝트의 중심위치를 설정한다.
    public EntityStats Stats => stats; // 읽기 전용 프로퍼티 
    public bool IsDead => Stats.CurrentHP != null && 
        Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f);  // 현재 채력이 Null이 아니고 기본값에서 0으로 변했으면 
    public Vector3 MiddlePoint => middlePoint != null  // middlePoint 변수에 등록해서 사용한다.
        ? middlePoint.position : Vector3.zero;
    public EntityBase Target { get; set; }
    protected virtual void Setup() // 가상 메소드(virtual)로 선언 이걸 상속 받는 친구들이 오버라이드 하게 하기 위해서 
    {
        Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value; // 최대 체력으로 초기화 게임 시작 할 때 
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return; // 죽었으면 반환

        Stats.CurrentHP.DefaultValue -= damage;
            

        if(Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f))
        {
            // 사망 처리 로직 
        }
    }
}
