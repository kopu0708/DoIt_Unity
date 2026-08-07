using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private SkillGad skillGad;
    [SerializeField]
    private Transform skillSpawnPoint;

    private PlayerBase owner;

    private void Awake()
    {
        owner = GetComponent<PlayerBase>();
        skillGad.Setup(owner, skillSpawnPoint);
    }

    private void Update()
    {
        //플레이어의 목표가 없거나 이동 중이면 모든 스킬 사용 불가
        if (owner.Target == null || owner.IsMoved == true) return;

        //기본 공격 스킬 업데이트
        skillGad.OnSkill();
    }
}
