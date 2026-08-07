using TMPro;
using UnityEngine;

public class UIDamageText : MonoBehaviour
{
    [SerializeField]
    private float arriveTime = 0.5f;
    private float percent = 0;
    private MovementRigidbody2D movement2D;
    private TextMeshPro text;

    public void Setup(string text, Color color)
    {
        movement2D = GetComponent<MovementRigidbody2D>();
        movement2D.MoveTo(new Vector3(Random.Range(-1f, 1f), 1, 0)); //이동방향을 무작위로 설정해서 포물성 방향으로 떨어지게 Rigdbody2D의 중력을 사용

        this.text = GetComponent<TextMeshPro>();
        this.text.text = text;
        this.text.color = color;

        Destroy(gameObject, arriveTime); // 시간이 지나면 오브젝트를 삭제 
    }

    private void Update() // 점점 흐려지는 효과
    {
        if (percent > 1) return;

        percent += Time.deltaTime / arriveTime;

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - percent);
    }
}
