using UnityEngine;

public class ProjectileLine : ProjectileBase
{
    [SerializeField]
    private LineRenderer lineRenderer;
    private float currentTime = 1f;
    private float duration = 0.5f;

    public override void Setup(EntityBase target, float damage)
    {
        base.Setup(target, damage);

        transform.rotation = Utils.RotateToTarget(transform.position,
            target.MiddlePoint);
    }

    public override void Process()
    {
        lineRenderer.material.mainTextureOffset += new Vector2(Time.time, 0);

        currentTime -= Time.deltaTime / duration;
        lineRenderer.startWidth = currentTime;
        lineRenderer.endWidth = currentTime;

        if (currentTime <= 0f) Destroy(gameObject);
    }
}
