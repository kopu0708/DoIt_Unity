using System;
using UnityEngine;

public class PlayerBase : EntityBase
{
    [SerializeField]
    private FollowTarget targetMark;
    // 현재 플레이어가 이동중인지
    // 스킬 사용 등 여러 곳에서 필요하므로 public 속성으로 정의

    // [이유] 여러 스크립트에서 이동중 여부를 체크해야 하기 때문에 지역 변수 대신 프로퍼티로 뺐다.
    // [참고] Update() 내부의 지역 변수는 그 함수 안에서만 존재하고, 외부에서 접근 불가능하다.
    public bool IsMoved { get; set; } = false; //초기값은 false 읽고 쓰기 프로퍼티 

    private void Awake()
    {
        base.Setup(); // EntityBase의 Setup() 메소드를 불러와 실행 
    }
    private void Update()
    {
        if (Target == null) targetMark.gameObject.SetActive(false);

        SearchTarget();
    }

    private void SearchTarget() //가장 가까운 대상을 찾아 공격하는 로직 
    {
        float closesDisSqr = Mathf.Infinity; // 가장 가까운 대상을 찾아야 하므로 가장 큰 값으로 설정

        foreach(var entity in EnemySpawner.Enemies) // 모든 적을 차례대로 탐색 
        {
            //가장 가까운 대상을 찾으므로 sqrMagnitude 사용
            float distance = (entity.transform.position - transform.position).sqrMagnitude; //적위치에서 내 위치 뺴기한 값을 벡터 제곱해서 distance에 저장
            if(distance < closesDisSqr)
            {
                closesDisSqr = distance;
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if(Target != null)
        {
            targetMark.SetTarget(Target.transform);
            targetMark.transform.position = Target.transform.position;
            targetMark.gameObject.SetActive(true);
        }
    }
}
