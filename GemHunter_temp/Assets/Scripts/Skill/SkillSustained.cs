using System.Collections.Generic;
using UnityEngine;

public class SkillSustained : SkillBase
{
    private float distanceToPlayer = 2f;
    private Transform parent;
    private List<GameObject> pickaxs = new List<GameObject>();

    public override void Setup(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        base.Setup(skillTemplate, owner, spawnPoint);

        // 곡괭이의 부모 오브젝트
        parent = GameObject.Find("Pickaxs").transform;
    }

    public override void OnLevelUp()
    {
        // 0에서 1로 늘어날때는 오브젝트만 생성
        if (currentLevel <= 1)
        {
            // 레벨 1이 되는 시점(최초 습득)에만 스탯에 설정된 개수만큼 곡괭이를 추가 생성
            AddPickax((int)GetStat(StatType.ProjectileCount).Value);

            // 현재 활성화된 모든 곡괭이 위치 재설정 (원형으로 균등 배치)
            int pickaxCount = parent.childCount;
            for (int i = 0; i < parent.childCount; ++i)
            {
                // 곡괭이 개수(pickaxCount)만큼 360도를 균등하게 나눠서, i번째 곡괭이의 각도를 구함
                // (예: 4개면 0도, 90도, 180도, 270도)
                float angle = (360 / pickaxCount) * i;
                // 위에서 구한 각도와, 플레이어로부터의 거리(distanceToPlayer)를 이용해
                // 원 둘레 위의 상대 좌표를 계산 (극좌표 → 직교좌표 변환)
                Vector3 position = Utils.GetPositionFromAngle(distanceToPlayer, angle);
                // 부모(플레이어) 위치를 기준으로 최종 위치를 재배치
                parent.GetChild(i).position = parent.position + position;
            }
            return;
        }

        // 공격 스킬 레벨업 시 공격력 등 스탯 갱신
        // List<> 내부 함수인 ForEach 각 요소마다 넘겨준 함수를 실행하는 함수이다. 
        skillTemplate.attackBuffStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue; 
        });

        foreach(var item in pickaxs)
        {
            item.GetComponent<ProjectileCollision2D>().Setup(
                null, GetStat(StatType.Damage).Value);
        }
    }

    private void AddPickax(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            GameObject clone = GameObject.Instantiate(skillTemplate.projectile, parent);
            clone.GetComponent<ProjectileCollision2D>().Setup(
                null, GetStat(StatType.Damage).Value);
            pickaxs.Add(clone);
        }
    }

    public override void OnSkill()
    {
        
    }
}
