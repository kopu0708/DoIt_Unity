using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using Unity.Behavior;

public class EnemyFSM : MonoBehaviour
{
    private EnemyBase owner;
    private NavMeshAgent navMeshAgent; // 적 이동 경로 설정과 이동 제어
    private BehaviorGraphAgent behaviorAgent; // 적 행동 제어
    private WeaponBase currentWeapon; // 현재 활성화된 무기

    private void Awake() 
    {
        owner = GetComponent<EnemyBase>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        currentWeapon = GetComponent<WeaponBase>();

        navMeshAgent.updateRotation = false; // 이동 방향으로 자동회전을 막아둔다.
        navMeshAgent.updateUpAxis = false; // 이건 2D 프로젝트인 경우 거의 필수로 끄라고 한다. 안끄면 2D 스프라이트가 이상하게 눕거나 기울어짐 
        currentWeapon.Setup(owner);
    }

    public void Setup(EntityBase target, GameObject[] wayPoints)
    {
        owner.Target = target;
        behaviorAgent.SetVariableValue("PatrolPoints", wayPoints.ToList()); // Blackboard에 선언한 patrolPoints 변수에 wayPoints 리스트 변수를 저장 
        behaviorAgent.SetVariableValue("Target", target.gameObject);
    }
}
