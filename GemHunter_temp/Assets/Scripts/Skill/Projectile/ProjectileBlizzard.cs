using UnityEngine;

public class ProjectileBlizzard : ProjectileGlobal
{
    public override void Process()
    {
        base.Process();

        // 눈보라 효과가 화면 전체에 계속 출력되도록 눈보라 위치를 플레이어 위치와 동일하게 설정
        transform.position = skillBase.Owner.transform.position;
        // AttackRate 마다 월드의 모든 적 데미지 입히기
        if(Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            for(int i = 0; i < EnemySpawner.Enemies.Count; i++)
            {
                if (EnemySpawner.Enemies[i] == null) continue;

                TakeDamage(EnemySpawner.Enemies[i]);
            }

            currentAttackRate = Time.time;
        }
    }
}
