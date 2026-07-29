using UnityEngine;
// 모든 스테이지가 같은 모양이라면 상관없지만, 스테이지마다 구조와 크기가 다르다면
// 스테이지에 따라 플레이어 이동할 수 있는 범위, 플레이어, 카메라의 시작 위치 등의 데이터를 저장해야 한다.
[CreateAssetMenu]

public class StageData : ScriptableObject
{
    [SerializeField]
    private Vector2 cameraLimitMin;
    [SerializeField]
    private Vector2 cameraLimitMax;

    [SerializeField]
    private Vector3 cameraStartPoint;

    [SerializeField]
    private Vector2 playerStartPoint;

    public Vector2 CameraLimitMin => cameraLimitMin; // 식 본문 멤버 (읽기 전용 프로퍼티)
                                                     // 오른쪽의 값을 반환한다. 즉 저장되어 있는 데이터를 다른 스크립트가 읽게 해줌
                                                     // 읽기 전용이기에 외부에서 수정이 불가능 
    public Vector2 CameraLimitMax => cameraLimitMax;
    public Vector3 CameraStartPoint => cameraStartPoint;
    public Vector2 PlayerStartPoint => playerStartPoint;

}