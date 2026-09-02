using UnityEngine;

public class SkillBuff : SkillBase
{
    public override void OnLevelUp()
    {
        skillTemplate.buffStatList.ForEach(stat =>
        {
            owner.Stats.GetStat(stat).BonusValue += stat.DefaultValue;
        });
    }

    public override void OnSkill()
    {
        
    }
}
