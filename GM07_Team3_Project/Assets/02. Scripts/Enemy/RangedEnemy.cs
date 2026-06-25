using UnityEngine;

public class RangedEnemy : Enemy
{
    private float attackTime;

    private void Update()
    {
        MoveToTarget();

        if (target == null)
        {
            return;
        }

        //원거리 공격 쿨타임 계산
        attackTime += Time.deltaTime;

        if (attackTime >= enemyData.AttackSpeed)
        {
            Shoot();
            attackTime = 0f;
        }

    }

    private void Shoot()
    {
        Debug.Log("원거리 공격");
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
