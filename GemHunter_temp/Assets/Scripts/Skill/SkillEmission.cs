using UnityEngine;

public class SkillEmission : SkillBase
{
    private float attackRate = 0.05f;
    private float currentAttackRate = 0;
    private int currentProjectileCount = 0;
    public override void OnLevelUp()
    {
        // 레벨이 0 에서 1로 갈 때는 스텟이 증가하지 않게
        if (currentLevel <= 1) return;

        // 공격 스킬 레벨업 시 공격력 등 스탯 갱신
        skillTemplate.attackBaseStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue;
        });
    }

    public override void OnSkill()
    {
        // 스킬이 사용가능한지 검색
        if( isSkillAvailable == true)
        {
            int maxCount = (int)GetStat(StatType.ProjectileCount).Value;

            // attackRate 주기로 발사체 생성
            if( Time.time - currentAttackRate > attackRate)
            {
                GameObject projectile = GameObject.Instantiate(
                    skillTemplate.projectile, spawnPoint.position, Quaternion.identity);

                // ProjectileStraight, ProjectileHoming도 연발은 가능하지만  3, 4번째 매개변수는 필요 없으므로 기존과 동일하게 처리
                if (projectile.TryGetComponent<ProjectileCubicHoming>(out var p))
                    p.Setup(owner.Target, GetStat(StatType.Damage).Value, maxCount, currentProjectileCount);
                else if (projectile.TryGetComponent<ProjectileQuadraticHoming>(out var p2))
                    p2.Setup(owner.Target, GetStat(StatType.Damage).Value, maxCount, currentProjectileCount);
                else projectile.GetComponent<ProjectileBase>().Setup(
                    owner.Target, GetStat(StatType.Damage).Value);

                currentProjectileCount++;
                currentAttackRate = Time.time;
            }

            // ProjectileCount 개수만큼 발사체를 생성한 후 쿨타임 초기화
            if(currentProjectileCount >= maxCount)
            {
                isSkillAvailable = false;
                currentProjectileCount = 0;
                currentCooldownTime = Time.time;
            }
        }
    }
}
