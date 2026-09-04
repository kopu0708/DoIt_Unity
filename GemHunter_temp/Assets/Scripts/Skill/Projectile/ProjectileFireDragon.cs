using UnityEngine;
using System.Collections.Generic;
public class ProjectileFireDragon : ProjectileGlobal
{
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float weight = 14f; // 이미지 크기에 따라 수정
    private float distancePerSecond = 20; // 초당 이동 거리 
    private float moveTime;
    private float t = 0f;
    private Vector3 start, end;
    private List<EntityBase> entities;

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);
        // 파이어 드래곤 이미지의 시작 위치와 목표 지점 설정
        start = new Vector3(0, stageData.CameraLimitMin.y - weight, 0);
        end = new Vector3(0, stageData.CameraLimitMax.y + weight, 0);
        transform.position = start;
        // 이동 시간 계산(맵 크기가 다르더라도 같은 속도로 이동)
        moveTime = Vector3.Distance(end, start) / distancePerSecond;
        // 현재 월드에 있는 모든 적 목록을 entities에 저장
        entities = new List<EntityBase>(EnemySpawner.Enemies);
        // entities를 y 위칫값이 낮은 순으로 정렬
        entities.Sort((a, b) =>
            a.transform.position.y.CompareTo(b.transform.position.y));
    }

    public override void Process()
    {
        // 도착 안했으면 위치를 부드럽게 이동하기 위해 Lerp함수 사용
        if(t < 1) // 아직 도착 안 했다면 (t가 1 미만이면)
        {
            t += Time.deltaTime / moveTime; // 진행률을 프레임마다 조금씩 증가
            transform.localPosition = Vector3.Lerp(start, end, t); // 그 진행률만큼 start, end 사이로 위치 이동 
        }
        else
        {
            // 필드에 있는 적을 공격하고 있지만 이미지가 목표에 도달했으므로 이미지만 비활성화
            spriteRenderer.enabled = false;
            // 필드에 있는 모든 적을 공격했으면 파이어 드래곤 제거
            if (entities.Count == 0) Destroy(gameObject);
        }

        // 공격 가능한 적이 없으면 나머지 코드는 처리하지 않음
        if (entities.Count == 0) return;

        // 현재 entities가 y 위치 기준으로 정렬되었으므로 
        // entities의 첫 번쨰 요소에 대해서만 파이어 드래곤 위치와 비교하여 처리
        if (entities[0] == null) entities.RemoveAt(0);
        else
        {
            if (entities[0].transform.position.y <= transform.position.y)
            {
                TakeDamage(entities[0]);
                entities.RemoveAt(0);
            }
        }
    }
}
