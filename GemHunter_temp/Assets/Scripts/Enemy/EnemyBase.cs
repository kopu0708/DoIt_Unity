using UnityEngine;

public class EnemyBase : EntityBase
{
    [SerializeField]
    private Transform hudPoint; // UI가 추적할 대상 
    [SerializeField]
    private GameObject uiPrefab;

    private EnemySpawner enemySpawner;
    private void Awake()
    {
        Setup(); //바로 호출 
    }

    public void Initialize(EnemySpawner enemySpawner,Transform parent)
    {
        this.enemySpawner = enemySpawner;
        GameObject clone = Instantiate(uiPrefab, parent); // 적의 체력을 출력하는 UI를 생성한다.
        clone.transform.localScale = Vector3.one; 
        clone.GetComponent<FollowTargetUI>().Setup(hudPoint); // hudPoint를 따라다니면서 위에 출력 체력을
        clone.GetComponentInChildren<UIHP>().Setup(this); // 적의 체력을 UI에 출력하도록 지정
    }
    protected override void Setup()
    {
        // 기본 체력은 DefaultValue에 할당하므로 추가 체력(BonusValue)만 설정
        Stats.GetStat(StatType.HP).BonusValue = 50 * (Stats.GetStat(StatType.Level).Value - 1); // 레벨에 따른 보너스 체력을 더해주는 식 
        base.Setup();
    }

    protected override void OnDie()
    {
        // 적은 레벨업 하지 않으므로 적 경험치 스탯만큼 플레이어 경험치 증가
        (Target as PlayerBase).AccumulationExp += Stats.CurrentExp.Value;
        //적 본인(this) 사망처리
        enemySpawner.Deactivate(this);
    }
}
