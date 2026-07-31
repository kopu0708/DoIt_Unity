using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ScaleEffect : MonoBehaviour
{
    [SerializeField]
    private float playTime = 0.1f;

    public void Play(Vector3 start, Vector3 end, UnityAction action = null) //UnityAction은 델리게이트    
    {
        StartCoroutine(ScaleAnimation(start, end, action));  // 코루틴을 실행 시킴
    }

    private IEnumerator ScaleAnimation(Vector3 start, Vector3 end, UnityAction action) // 코루틴 내부 로직은 private로 
    {
        float percent = 0;

        while(percent < 1) //percent가 1.0이 될 때까지 반복문을 호출 
        {
            percent += Time.deltaTime / playTime;

            transform.localScale = Vector3.Lerp(start, end, percent); // playTime 동안 start 부터 end까지 변한다. 반복문이 끝나면 

            yield return null; 
        }
        // action에 메서드가 있는지 검사하고 있으면 해당 메소드 호출
        action?.Invoke(); //action이 null 이면 실행 안함 null이 아니면 Invoke() 실행 이벤트나 콜백 처리에서 자주 나오는 패턴
                          //Invoke()는 델리게이트를 실행시키는 메소드  
    }
}
