using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public static class FadeEffect // 페이드 효과는 게임 전반으로 사용되므로 정적 클래스와 메서드로 정의해 인스턴스 변수를 생성하지 않고 호출할 수 있게 한다.
{
    // 다른 곳에서도 Fade 효과를 주고 싶으면 이 메서드를 복사 붙여 넣기 해주고 SpriteRenderer만 원하는 자료형으로 변경해주자 
    public static IEnumerator Fade(SpriteRenderer target, float start, float end,
        float fadeTime = 1f, UnityAction action = null) // UnityAction은 유니티에서 사용되는 범용 델리게이트 타입이다. 페이드 효과가 끝나고 원하는 메소드를 호출하기 위해 매개변수로 받는다.
    {                                                   // 알파값을 변경할 대상, 알파의 시작값과 종료값 효과를 재생하는 시간을 매개변수로 받는다.
        if (target == null) yield break;

        float percent = 0;

        while(percent < 1) // 퍼센트 게이지 
        {
            percent += Time.deltaTime / fadeTime;

            Color color = target.color;
            color.a = Mathf.Lerp(start, end, percent);  // 부드러운 처리 
            target.color = color;

            yield return null;
        }

        action?.Invoke();
    }
}
