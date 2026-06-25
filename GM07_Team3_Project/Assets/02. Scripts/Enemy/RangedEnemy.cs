using UnityEngine;

public class RangedEnemy : Enemy
{
    private float attackTime;

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        
        // 적 현재위치와 타겟 위치 사이 실제거리 계산 후 distance에 저장
        float distance = 
            Vector3.Distance(transform.position, target.position);

        if (distance > enemyData.AttackRange)
        {
            MoveToTarget();

            // NavMeshAgent 깨워서 다시 작동
            if (agent != null)
            {
                agent.isStopped = false;
            }
        }

        else
        {
            if (agent != null)
            {
                agent.isStopped = true;
            }

            attackTime += Time.deltaTime;

            if (attackTime >= enemyData.AttackSpeed)
            {
                Shoot();
                attackTime = 0f;
            }
        }
    }

    private void Shoot()
    {
        if (target == null)
        {
            return;
        }
        EnemyBullet bullet = Instantiate(enemyData.BulletPrefab,
            transform.position, Quaternion.identity);

        Vector3 direction = target.position - transform.position;

        bullet.Initialize(direction);

    }

    //플레이어에 부딪히면 데미지
    private void OnTriggerStay(Collider other)
    {
        // 충돌 물체 태그가 플레이어인지 확인하는 용도
        if (!other.CompareTag("Player"))
        {
            return;
        }

        IDamageable damageable =
            other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(enemyData.AttackPower);
        }
    }
}
