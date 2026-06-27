using UnityEngine;

public class RangedEnemy : Enemy
{
    private float attackTimer;

    private float contactTimer;

    private void Update()
    {
        if (target == null || enemyData == null)
        {
            return;
        }

        Vector3 toTarget = target.position - transform.position;
        float sqrDistance = toTarget.sqrMagnitude;

        float sqrAttackRange = 
            enemyData.AttackRange * enemyData.AttackRange;


        // 추적
        if (sqrDistance > sqrAttackRange)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
            }

            MoveToTarget();
            return;
        }

        // 공격
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        Vector3 dir = toTarget;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= enemyData.AttackSpeed)
        {
            Shoot();
            attackTimer = 0f;
        }
    }
    private void Shoot()
    {
        Vector3 spawnPosition =
            transform.position + Vector3.up * 1.5f;

        EnemyBullet bullet = EnemyBulletPool.Instance.Get();

        bullet.transform.position = spawnPosition;

        Vector3 direction =
            (target.position + Vector3.up * 1f) - spawnPosition;

        bullet.Initialize(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || enemyData == null)
        {
            return;
        }

        // 부딪혔을때 즉시 데미지 1회
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(enemyData.AttackPower);
        }

        contactTimer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || enemyData == null)

        {
            return;
        }

        contactTimer += Time.deltaTime;

        // 부딪힌 동안 1초마다 지속 데미지
        if (contactTimer < 1f)

        {
            return;
        }

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(enemyData.AttackPower);
        }

        contactTimer = 0f;
    }

}
