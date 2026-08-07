using UnityEngine;
using System.Collections;

public enum EnemyState { None = -1 ,Attack, }
public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;

    private EnemyBase owner;

    private EnemyState enemyState;

    private void Awake() 
    {
        owner = GetComponent<EnemyBase>();   

        ChangeState(EnemyState.Attack); // Awake() 메서드에서 현재상태를 공격으로 설정해 Attack() 코루틴 메소드를 호출 
    }

    public void Setup(EntityBase target)
    {
        owner.Target = target;
    }

    public void ChangeState(EnemyState newState)
    {
        // 열거형 변수.ToString()은 열거형으로 정의한 변수 이름을 문자열로 반환한다.
        // ex) enemyState가 현재 EnemyState.Idle 이면 "Idle" 문자열을 반환
        // 이를 이용해 열거형 이름과 코루틴 이름을 일치시켜 
        // 열거형 변수에 따라 코루틴 함수를 재생시켜 제어할 수 있다.

        // 이전에 재생 중이던 상태 종료
        StopCoroutine(enemyState.ToString());
        // 상태 변경
        enemyState = newState;
        // 새로운 상태 재생
        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Attack()
    {
        var wait = new WaitForSeconds(owner.Stats.GetStat(StatType.CooldownTime).Value); //쿨타임이 돌때마다.

        while (true)
        {
            yield return wait;

            Vector3 target = owner.Target.MiddlePoint;  // 타겟의 중간 지점으로 
            GameObject clone = Instantiate(projectilePrefab); // 프리팹을 생성해 
            clone.transform.position = projectileSpawnPoint.position; // 직선으로 날린다. 
            clone.GetComponent<EnemyProjectile>().Setup(target,
                owner.Stats.GetStat(StatType.Damage).Value); //그를 위해서 Setup메소드 호출 
        }
    }
}
