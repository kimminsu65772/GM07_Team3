using UnityEngine;

public class MeleeEnemy : Enemy
{
    private void Update()
    {
        MoveToTarget();
    }

    // 공격 쿨타임 계산용 변수
    private float attackTimer;

    // 공격 범위 안에 플레이어가 있는 동안 공격 수행용 메서드
    private void OnTriggerStay(Collider other)
    {
        // 충돌 물체 태그가 플레이어인지 확인하는 용도
        if (!other.CompareTag("Player"))
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
        }

        attackTimer = 0f;
    }

}
