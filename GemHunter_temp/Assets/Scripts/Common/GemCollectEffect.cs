using UnityEngine;

public class GemCollectEffect : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private GemCollecter gemCollecter;
    private RectTransform uiElement; // 보석이 이동할 목표 ui
    private Vector3 start, end, point1;
    private float percent, duration;
    private readonly float minDuration = 0.5f, maxDuration = 2f, r = 3f;

    private void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(GemCollecter gemCollecter, RectTransform uiElement)
    {
        this.gemCollecter = gemCollecter;
        this.uiElement = uiElement;
        percent = 0f;
        start = transform.position;
        duration = Random.Range(minDuration, maxDuration);
        point1 = Utils.GetNewPoint(start, Random.Range(0, 360), r);

        // duration 시간 동안 서서히 사라짐
        StartCoroutine(FadeEffect.Fade(spriteRenderer, 1, 0, duration));
    }

    private void Update()
    {
        if(percent >= 1f)
        {
            gemCollecter.OnGemCollect(gameObject);
            return;
        }

        UpdateEndPoint();
        percent += Time.deltaTime / duration;
        transform.position = Utils.QuadraticCurve(start, point1, end, percent);
    }

    private void UpdateEndPoint()
    {
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, uiElement.position);

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, screenPoint, mainCamera, out Vector3 worldPoint))
            end = worldPoint;
    }
}
