using UnityEngine;

public class EnemyBase : EntityBase
{
    [SerializeField]
    private Transform hudPoint; // UI가 추적할 대상 
    [SerializeField]
    private GameObject uiPrefab;

    private void Awake()
    {
        Setup(); //바로 호출 
    }

    public void Initialize(Transform parent)
    {
        GameObject clone = Instantiate(uiPrefab, parent); // 적의 체력을 출력하는 UI를 생성한다.
        clone.transform.localScale = Vector3.one; 
        clone.GetComponent<FollowTargetUI>().Setup(hudPoint); // hudPoint를 따라다니면서 위에 출력 체력을
        clone.GetComponentInChildren<UIHP>().Setup(this); // 적의 체력을 UI에 출력하도록 지정
    }
    protected override void Setup()
    {
        stats.MaxHp = 100 + 50 * (stats.level - 1); // 기본 100 레벨당 50 추가 
        base.Setup();
    }
}
