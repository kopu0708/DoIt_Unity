using UnityEngine;

public class PlayerBase : EntityBase
{
    // 현재 플레이어가 이동중인지
    // 스킬 사용 등 여러 곳에서 필요하므로 public 속성으로 정의

    // [이유] 여러 스크립트에서 이동중 여부를 체크해야 하기 때문에 지역 변수 대신 프로퍼티로 뺐다.
    // [참고] Update() 내부의 지역 변수는 그 함수 안에서만 존재하고, 외부에서 접근 불가능하다.
    public bool IsMoved { get; set; } = false; //초기값은 false 읽고 쓰기 프로퍼티 

    private void Awake()
    {
        base.Setup(); // EntityBase의 Setup() 메소드를 불러와 실행 
    }
}
