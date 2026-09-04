using UnityEngine;

public class ProjectileLightningStrike : ProjectileGlobal
{
    [SerializeField]
    private GameObject projectile;

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);
    }

    public override void Process()
    {
        for( int i = 0; i < EnemySpawner.Enemies.Count; ++i)
        {
            if (EnemySpawner.Enemies[i] == null) continue;

            // Enemies[i] 적을 강타하는 번개 효과 생성
            Instantiate(projectile, EnemySpawner.Enemies[i].MiddlePoint,
                Quaternion.identity);
            TakeDamage(EnemySpawner.Enemies[i]);
        }

        Destroy(gameObject);
    }
}
