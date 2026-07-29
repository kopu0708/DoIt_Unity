using TreeEditor;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target; // 추적할 대상은 플레이어,적,카메라,조명 등 다양한 오브젝트 
    [SerializeField]
    private bool x, y, z; // 추적할 축(x, y, z 개별 on/off)

    private void Update()
    {
        if (target == null) return;  // 방어 코드 

        // 활성화 축은 target 위치, 비활성화 축은 자기 자신의 위치로 설정
        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),  // x가 true 이면 목표의 위치를 추적, false 이면 자신의 위치를 사용 이하 동일
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
}
