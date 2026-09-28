using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform rectBackground; // 컨트롤러 배경
    [SerializeField]
    private RectTransform rectController; // 터치 정보에 따라 위치가 바뀌는 컨트롤러

    // 터치 위치를 외부로 보내고자 지역 변수가 아닌 멤버 변수로 선언
    private Vector2 touchPosition;

    // x, y 방향값을 외부에서 열람할 수 있도록 Get 전용 속성 정의
    public float Horizontal => touchPosition.x;
    public float Vertical => touchPosition.y;

    private void Awake()
    {
        rectBackground.gameObject.SetActive(false);
    }

    /// <summary>
    /// 터치하는 순간 1회 호출한다.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        rectBackground.gameObject.SetActive(true);

        // 터치한 위치에 컨트롤러 위치 변경
        rectBackground.transform.position = eventData.position;
    }

    ///<summary>
    ///터치 상태로 드래그할 때 매 프레임 호출한다.
    ///</summary>
    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;

        // 터치 위치(eventData.position)가 BackGround 오브젝트(rectBackground)에서 
        // 얼마나 떨어졌는지 계산해(rectBackground 중심위치) touchPosition에 저장
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectBackground, eventData.position, eventData.pressEventCamera,
            out touchPosition))
        {
            // touchPosition값 연산
            // touchPosition을 컨트롤러 배경(rectBackGround) 크기로 나누고 2를 곱합
            touchPosition.x = (touchPosition.x / rectBackground.sizeDelta.x * 2);
            touchPosition.y = (touchPosition.y / rectBackground.sizeDelta.y * 2);

            // touchPosition값 정규화 (-1 ~1)
            // 현재 터치 위치가 가상 컨트롤러 배경(rectBackground) 바깥일 떄 
            // -1 ~ 1 보다 큰 값이므로 정규화
            touchPosition = (touchPosition.magnitude > 1) ?
                touchPosition.normalized : touchPosition;

            // 가상 컨트롤러 이동 
            rectController.anchoredPosition = new Vector2(
                touchPosition.x * rectBackground.sizeDelta.x * 0.5f,
                touchPosition.y * rectBackground.sizeDelta.y * 0.5f);
        }
    }

    /// <summary>
    /// 터치를 종료하는 순간 1회 호출
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치 종료 시 이미지 위치를 가운데로 이동
        rectController.anchoredPosition = Vector2.zero;
        // 터치 종료 시 touchPosition 값도 (0,0)으로 초기화
        touchPosition = Vector2.zero;

        rectBackground.gameObject.SetActive(false);
    }
}
