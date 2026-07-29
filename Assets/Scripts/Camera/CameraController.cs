using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;

    private void LateUpdate()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, stageData.CameraLimitMin.x, stageData.CameraLimitMax.x);

        position.y = Mathf.Clamp(position.y, stageData.CameraLimitMin.y, stageData.CameraLimitMax.y);
        //Clamp는 max와 min 사이의 값을 넘지 않도록 보정해주는 함수 
        transform.position = position; //바뀐 값으로 다시 덮어 씌워줌 

        // 벡터는 구조체(값 타입)이기 때문에, transform.position이 반환한 결과물(복사본)의 필드를 직접 수정할 수 없음
        // 그래서 변수에 복사하고 , 복사본을 수정하고, 수정된 복사본을 통째로 다시 대입 하는 방식
    }
}
