using UnityEngine;

public class ProjectileStraight : ProjectileBase
{
    public override void Setup(EntityBase target, float damage)
    {
        base.Setup(target, damage);

        // 발사체를 목표 방향으로 회전
        transform.rotation = Utils.RotateToTarget(transform.position, target.MiddlePoint, 90);

        //발사체 이동 방향
        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);

    }

    public override void Process()
    {
        
    }
}
