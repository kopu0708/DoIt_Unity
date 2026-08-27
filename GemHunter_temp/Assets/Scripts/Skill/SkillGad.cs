using System.Data.Common;
using UnityEngine;

public class SkillGad : MonoBehaviour
{
    [SerializeField]
    private ProjectileGad projectile;

    private float currentCooldownTime;
    private Transform spawnPoint;
    private PlayerBase owner;

    public void Setup(PlayerBase owner, Transform spawnPoint)
    {
        this.owner = owner;
        this.spawnPoint = spawnPoint;
    }

    public void OnSkill()
    {
        //스킬이 사용할 수 있는 상태인지 검사(쿨타임)
        if(Time.time -currentCooldownTime >
            owner.Stats.GetStat(StatType.CooldownTime).Value)
        {
            var result = CalculateDamage();
            ProjectileGad gad = Instantiate(projectile, spawnPoint.position,
                Quaternion.identity);
            gad.Setup(owner.Target, result.Item1, result.Item2);

            currentCooldownTime = Time.time;
        }
    }

    private (float,bool) CalculateDamage()
    {
        bool isCriticalHit = Random.value <
            owner.Stats.GetStat(StatType.CriticalChance).Value;

        float damage = owner.Stats.GetStat(StatType.Damage).Value;

        if (isCriticalHit)
            return (damage * owner.Stats.GetStat(StatType.CriticalMultiplier).Value, isCriticalHit);
        else
            return (damage, isCriticalHit);
    }
}
