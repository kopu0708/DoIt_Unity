using System;
using UnityEngine;

public class PlayerBase : EntityBase
{
    [SerializeField]
    private FollowTarget targetMark;
    [SerializeField]
    private LevelData levelData; // 인게임 레벨업 경험치 테이블 정보
    [SerializeField]
    private SkillSystem skillSystem; // 레벨업 할 때 스킬을 배울 수 있도록

    private float expAmount = 2f; // 매 프레임 흡수하는 경험치 양
    // 현재 플레이어가 이동중인지
    // 스킬 사용 등 여러 곳에서 필요하므로 public 속성으로 정의

    // [이유] 여러 스크립트에서 이동중 여부를 체크해야 하기 때문에 지역 변수 대신 프로퍼티로 뺐다.
    // [참고] Update() 내부의 지역 변수는 그 함수 안에서만 존재하고, 외부에서 접근 불가능하다.
    public bool IsMoved { get; set; } = false; //초기값은 false 읽고 쓰기 프로퍼티 

    // 적을 물리치고 축적한 경험치
    public float AccumulationExp { get; set; } = 0f;

    private void Awake()
    {
        base.Setup(); // EntityBase의 Setup() 메소드를 불러와 실행 

        Stats.CurrentExp.DefaultValue = 0f;
        Stats.CurrentExp.OnValueChanged += IsLevelUP;
        Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[0];
    }
    private void Update()
    {
        if (Target == null) targetMark.gameObject.SetActive(false);

        SearchTarget();
        Recovery();
        UpdateEXP();
    }

    private void UpdateEXP()
    {
        if (Mathf.Approximately(AccumulationExp, 0f) ||  // 매 프레임마다 실행되니깐 스킬을 선택중이거나, 들어온 경험치가 없으면 return 시켜준다.
            skillSystem.IsSelectSkill == true) return;

        float getEXP = AccumulationExp > expAmount ? expAmount : AccumulationExp;
        AccumulationExp -= getEXP; // 축적 경험치에서 getEXP만큼 소모
        Stats.CurrentExp.DefaultValue += getEXP; // 경험치를 getEXP만큼 증가
    }

    private void IsLevelUP(Stat stat, float prev, float current)
    {
        // 경험치가 최대가 아니면 return
        if (!Mathf.Approximately(Stats.CurrentExp.Value,
            Stats.GetStat(StatType.Experience).Value)) return;

        // 플레이어 레벨업(현재는 최대 레벨일 때 UI를 출력하거나 하지 않음)
        Stats.GetStat(StatType.Level).DefaultValue++;

        // 현재 경험치  설정 (레벨업에 사용한 만큼 감소)
        Stats.CurrentExp.DefaultValue -= Stats.GetStat(StatType.Experience).Value;

        // 최대 경험치 설정
        if (Stats.GetStat(StatType.Level).Value < levelData.MaxExperience.Length)
            Stats.GetStat(StatType.Experience).DefaultValue =
                levelData.MaxExperience[(int)Stats.GetStat(StatType.Level).Value - 1];
        else
            Stats.GetStat(StatType.Experience).DefaultValue =
                levelData.MaxExperience[levelData.MaxExperience.Length - 1];

        // 레벨업 할 때 스킬을 선택할 수 있도록 스킬 선택 팝업 창 출력
        skillSystem.StartSelectSkill();
    }

    private void SearchTarget() //가장 가까운 대상을 찾아 공격하는 로직 
    {
        float closesDisSqr = Mathf.Infinity; // 가장 가까운 대상을 찾아야 하므로 가장 큰 값으로 설정

        foreach(var entity in EnemySpawner.Enemies) // 모든 적을 차례대로 탐색 
        {
            //가장 가까운 대상을 찾으므로 sqrMagnitude 사용
            float distance = (entity.transform.position - transform.position).sqrMagnitude; //적위치에서 내 위치 뺴기한 값을 벡터 제곱해서 distance에 저장
            if(distance < closesDisSqr)
            {
                closesDisSqr = distance;
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if(Target != null) // 타겟이 있다면 SetTarget메서드 호출 
        {
            targetMark.SetTarget(Target.transform);
            targetMark.transform.position = Target.transform.position; //targetMark의 오브젝트 위치를 Target위치로 설정 
            targetMark.gameObject.SetActive(true); // 월드에 출력 
        }
    }

    private void Recovery()
    {
        // 체력 회복 
        if (Stats.CurrentHP.DefaultValue < Stats.GetStat(StatType.HP).Value)
            Stats.CurrentHP.DefaultValue += Time.deltaTime * Stats.GetStat(StatType.HPRecovery).Value;

        else
            Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
    }

    protected override void OnDie()
    {
        Logger.Log("플레이어 사망 처리");
    }
}
