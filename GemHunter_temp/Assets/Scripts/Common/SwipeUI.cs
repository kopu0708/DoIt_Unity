using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollBar; // scrollbar 위치를 바탕으로 현제 페이지 검사
    [SerializeField]
    private float swipeTime = 0.2f; // 페이지가 스와이프되는 시간
    [SerializeField]
    private float swipeDistance = 50.0f; // 스와이프할 때 움직여야 하는 최소 거리

    private float[] scrollPageValues; // 각 페이지의 위칫값(0.0~1.0)
    private float valueDistance = 0; // 각 페이지 사이의 거리
    private int currentPage = 0; // 현재 페이지
    private int maxPage = 0; //최대 페이지
    private float startTouchX; // 터치 시작 위치
    private float endTouchX; // 터치 종료 위치
    private bool isSwipeMode = false; // 현재 스와이프 중인지 체크 

    // 현재 페이지 index 정보
    public int CurrentPage => currentPage;

    private void Start()
    {
        // 최대 페이지 수
        maxPage = transform.childCount;
        // 스크롤되는 페이지의 각 value값을 저장하는 배열 메모리 할당 
        scrollPageValues = new float[transform.childCount];
        // 스크롤되는 페이지 사이 거리
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // 스크롤되는 페이지 각 value 위치 설정 [0 <= value <= 1]
        for(int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
        SetScrollBarValue(0);
    }

    private void Update()
    {
        UpdateInput();
    }

    public void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollBar.value = scrollPageValues[index];
    }

    private void UpdateInput()
    {
        //스와이프 중이면 터치 불가
        if (isSwipeMode == true) return;

        #if UNITY_EDITOR
        if(Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                startTouchX = Mouse.current.position.ReadValue().x; // 클릭 시작점
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                endTouchX = Mouse.current.position.ReadValue().x; // 클릭 종료지점
                UpdateSwipe();
            }
        }
        #elif UNITY_ANDROID
        if(Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.touches[0];
            if (touch.press.wasPressedThisFrame)
            {
                startTouchX = touch.position.ReadValue().x; // 시작점(방향 구분)
            }
            else if (touch.press.wasReleasedThisFrame)
            {
                endTouchX = touch.position.ReadValue().x; // 종료점 (방향 구분)
                UpdateSwipe();
            }
        }
        #endif
    }

    private void UpdateSwipe()
    {
        // 너무 짧은 거리를 움직였다면 스와이프 하지 않음
        if(Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            // 원래 페이지로 스와이프해서 돌아감
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        // 스와이프 방향
        bool isLeft = startTouchX < endTouchX;
        if(isLeft == true) // 이동 방향이 왼쪽일 때
        {
            if (currentPage == 0) return; // 현재 페이지가 왼쪽 끝이면 종료

            currentPage--; // 왼쪽으로 이동하고자 현재 페이지 1 감소
        }
        else // 이동 방향이 오른쪽일 때
        {
            if (currentPage == maxPage - 1) return; // 페이지가 오른쪽 끝이면 종료
            currentPage++; // 오른쪽으로 이동하고자 현재 페이지 1 증가
        }
        // currentIndex번쨰 페이지로 스와이프해서 이동
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    /// <summary>
    /// 페이지를 한 장 옆으로 넘기는 스와이프 효과를 재생한다.
    /// </summary>
    private IEnumerator OnSwipeOneStep(int index)
    {
        float start = scrollBar.value;
        float percent = 0;

        isSwipeMode = true;

        while(percent < 1)
        {
            percent += Time.deltaTime / swipeTime;
            scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);
            yield return null;
        }

        isSwipeMode = false;
    }
}
