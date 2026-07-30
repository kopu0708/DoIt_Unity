using UnityEngine;

public class EnemyBase : EntityBase
{
    private void Awake()
    {
        Setup(); //바로 호출 
    }

    protected override void Setup()
    {
        stats.MaxHp = 100 + 50 * (stats.level - 1); // 기본 100 레벨당 50 추가 
        base.Setup();
    }
}
