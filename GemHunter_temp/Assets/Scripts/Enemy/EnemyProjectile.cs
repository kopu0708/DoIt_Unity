using UnityEngine;

[RequireComponent(typeof(MovementRigidbody2D))] // 적 발사체가 생성한 후 MovementRigidbody2D 를 이용해서 이동하도록 함께 추가하게 선언 
public class EnemyProjectile : MonoBehaviour
{
    private MovementRigidbody2D movementRigidbody2D;
    private ScaleEffect scaleEffect; 
    private float damage;

   public void Setup(Vector3 target, float damage) // 이동 방향을 Setup 메소드에서 1회만 설정해서 직진만 하게 
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        scaleEffect = GetComponent<ScaleEffect>();   
        this.damage = damage;

        //발사체 크기를 20%에서 100% 확대
        scaleEffect.Play(transform.localScale * 0.2f, transform.localScale);
        //발사체 이동 방향 설정
        movementRigidbody2D.MoveTo((target - transform.position).normalized);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall")) // 벽에 닿으면 그냥 파괴
        {
            Destroy(gameObject);    
        }

        else if(collision.CompareTag("Player") && 
            collision.TryGetComponent<EntityBase>(out var entity))  //플레이어 태그가 달려있고 엔티티가 달려있으면 entity의 체력을 damage만큼 깍음
        {   
            entity.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
