using UnityEngine;

public enum DestroyType { None = -1, Collision = 0, Indestructible, } // Collision은 파괴 되는거, Indestructible은 유지 되는거
public enum AttackType { Single, Multiple }

public class ProjectileCollision2D : MonoBehaviour
{
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private UIDamageText damageText;
    [SerializeField]
    private DestroyType destroyType = DestroyType.None;
    [SerializeField]
    private AttackType attackType = AttackType.Single;
    [SerializeField]
    private bool isIgnoreWall = false;

    private EntityBase target;
    private float damage;

    public void Setup(EntityBase target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }
    public void SetTarget(EntityBase target) => this.target = target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && isIgnoreWall == false) // 벽 통과가 가능하면 true로 바꿔야한다.
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Enemy") && 
            collision.TryGetComponent<EntityBase>(out var entity)) // 적에 닿았고 EntityBase 컴포넌트가 붙어 있다면 entity 객체 즉시 선언후 사용
        {
            if (attackType == AttackType.Single && entity != target) return;

            if(hitEffect != null)
            {
                Instantiate(hitEffect, collision.transform.position,
                    Quaternion.identity);
            }
            if(damageText != null)
            {
                UIDamageText clone = Instantiate(damageText, collision.transform.position, Quaternion.identity);
                clone.Setup(damage.ToString("F0"), Color.white);
            }

            entity.TakeDamage(damage);

            if(destroyType == DestroyType.Collision)
            {
                Destroy(gameObject);
            }
        }
    }

}
