using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // 클래스 정의 앞에 이걸 붙이면 이걸 등록할때 자동으로 해당 컴포넌트를 붙여준다, 
public class MovementRigidbody2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rigid2D;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector3 direction)
    {
        rigid2D.linearVelocity = direction * moveSpeed;
    }
}
