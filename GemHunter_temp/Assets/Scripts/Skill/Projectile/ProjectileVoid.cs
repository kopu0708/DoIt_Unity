using UnityEngine;

public class ProjectileVoid : ProjectileGlobal
{
    [SerializeField]
    private GameObject projectile;

    private float radius = 1f;

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);
        // 암흑 공격 본체 이동 방향 설정
        Vector3 targetPosition = skillBase.Owner.Target.MiddlePoint;
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        movementRigidbody2D.MoveTo((targetPosition - transform.position).normalized);
    }

    public override void Process()
    {
        base.Process();

        if(Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            for(int i = 0; i<skillBase.GetStat(StatType.ProjectileCount).Value; ++i)
            {
                // 암흑 공격 발사체 생성과 위치 설정
                float angle =
                    360 / skillBase.GetStat(StatType.ProjectileCount).Value * i;
                Vector3 position = transform.position +
                    Utils.GetPositionFromAngle(radius, angle);
                GameObject clone = Instantiate(projectile, position,
                    Quaternion.identity);
                // 암흑 공격 발사체 이동 방향 설정
                Vector3 direction = (clone.transform.position -
                    transform.position).normalized;
                clone.GetComponent<MovementRigidbody2D>().MoveTo(direction);
                // 암흑 공격 발사체 공격력 설정
                clone.GetComponent<ProjectileCollision2D>().Setup(
                    skillBase.Owner.Target, damage);
            }

            currentAttackRate = Time.time;
        }
    }
}
