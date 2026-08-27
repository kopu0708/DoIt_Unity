using UnityEngine;


public abstract class SkillBase : MonoBehaviour
{
    protected SkillTemplate skillTemplate; // 스킬 정보
    protected PlayerBase owner; // 스킬 소유자 
    protected int currnetLevel = 0; // 현재 스킬 레벨 

    // 외부에서 접근 할 수 있도록 Get 속성으로 정의
    public string SkillName => skillTemplate.skillName;
    public SkillType SkillType => skillTemplate.skillType;
    public SkillElement Element => skillTemplate.element;
    public string Description => skillTemplate.description;
    public int CurrentLevel => currnetLevel;
    public bool IsMaxLV => currnetLevel == skillTemplate.maxLevel;

    public virtual void Setup(SkillTemplate skillTemplate, PlayerBase owner)
    {
        this.skillTemplate = skillTemplate;
        this.owner = owner;
    }

    public void TryLevelUP()
    {
        if (IsMaxLV)
        {
            Logger.Log($"[{SkillName}] 스킬 최고 레벨 도달");
            return;
        }

        currnetLevel++;

        OnLevelUp();
    }

    public abstract void OnLevelUp(); // 스킬 레벨업 시 1회 호출 
    public abstract void OnSkill(); // 스킬 사용시 호출    
}
