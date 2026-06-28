using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    // 공격 쿨타임 계산용 변수
    private float attackTimer;

    private void Update()
    {
        MoveToTarget();

        // NavMeshAgent가 움직이는 실제속도 구하기
        if (agent != null && anim != null)
        {
            float currentSpeed = agent.velocity.magnitude;
            
            // 애니메이터의 Speed 파라미터 갱신
            anim.SetFloat("Speed", currentSpeed);
        }
    }

    // 공격 범위 안에 플레이어가 있는 동안 공격 수행용 메서드
    private void OnTriggerStay(Collider other)
    {
        // 충돌 물체 태그가 플레이어인지 확인하는 용도
        if (!other.CompareTag("Player") || enemyData == null)
        {
            return;
        }
        
        attackTimer += Time.deltaTime;

        if (attackTimer < enemyData.AttackSpeed)
        {
            return;
        }

        IDamageable damageable =
            other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(enemyData.AttackPower);

            
            /* 공격 애니 트리거
            if (anim != null)
            {
                anim.SetTrigger("Attack");
            }
            추후 사용하게되면 꺼냄 */
        }

        attackTimer = 0f;
    }

    // 공격범위 들어올때 타이머 초기화
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attackTimer = 0f;
        }
    }

    // 플레이어가 공격범위 빠질때 타이머 초기화
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attackTimer = 0f;
        }
    }

}
