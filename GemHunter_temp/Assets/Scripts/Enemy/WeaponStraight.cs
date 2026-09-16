using UnityEngine;

public class WeaponStraight : WeaponBase
{
    public override void OnAttack()
    {
        GameObject clone = GameObject.Instantiate(projectilePrefab,
            projectileSpawnPoint.position, Quaternion.identity);
        clone.GetComponent<EnemyProjectile>().Setup(owner.Target.MiddlePoint, damage);
    }
}
