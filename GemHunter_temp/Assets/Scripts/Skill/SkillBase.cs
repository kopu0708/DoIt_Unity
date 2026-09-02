using UnityEngine;
using System.Linq;

public abstract class SkillBase 
{
    protected SkillTemplate skillTemplate; // 스킬 정보
    protected PlayerBase owner; // 스킬 소유자 
    protected Transform spawnPoint; // 스킬 발사 위치
    protected int currentLevel = 0; // 현재 스킬 레벨 

    protected float currentCooldownTime = 0;
    protected bool isSkillAvailable = false;

    // 외부에서 접근 할 수 있도록 Get 속성으로 정의
    public string SkillName => skillTemplate.skillName;
    public SkillType SkillType => skillTemplate.skillType;
    public SkillElement Element => skillTemplate.element;
    public string Description => skillTemplate.description;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLV => currentLevel == skillTemplate.maxLevel;

    // 공격 스킬 전용(공격력, 쿨타임, 발사체 개수 같은 스텟)
    private Stat[] stats;
    public Stat GetStat(Stat stat)
        => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public Stat GetStat(StatType statType)
        => stats.FirstOrDefault(s => s.StatType == statType);

    public virtual void Setup(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        this.skillTemplate = skillTemplate;
        this.owner = owner;
        this.spawnPoint = spawnPoint;

        // 공격 스킬이라면 필요한 스탯 설정(공격력, 쿨타임, 발사체 개수 등)
        if(SkillType != SkillType.Buff)
        {
            stats = new Stat[skillTemplate.attackBaseStats.Count];
            for(int i = 0; i<stats.Length; ++i)
            {
                stats[i] = new Stat();
                stats[i].CopyData(skillTemplate.attackBaseStats[i]);
            }
        }
    }

    public void TryLevelUP()
    {
        if (IsMaxLV)
        {
            Logger.Log($"[{SkillName}] 스킬 최고 레벨 도달");
            return;
        }

        currentLevel++;

        OnLevelUp();
    }

    public void IsSkillAvailable()
    {
        // 레벨이  0이거나 버프 또는 지속 스킬이라면 사용할 수 있는 상태 아님
        if (CurrentLevel == 0 || SkillType == SkillType.Buff
            || SkillType == SkillType.Sustained) return;

        if(Time.time - currentCooldownTime > GetStat(StatType.CooldownTime).Value)
        {
            isSkillAvailable = true;
        }
    }
    public abstract void OnLevelUp(); // 스킬 레벨업 시 1회 호출 
    public abstract void OnSkill(); // 스킬 사용시 호출    
}
