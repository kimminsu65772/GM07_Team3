using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Target")]
    [SerializeField] private Transform target;

    protected override void OnEnable()
    {
        base.OnEnable();
        FindPlayer();
    }

    private void Update()
    {
        MoveToTarget();
    }

    private void FindPlayer()
    {
        if (target != null)
        {
            return;
        }

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
    }

    private void MoveToTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 direction =
            (target.position - transform.position).normalized;

        transform.position += direction * enemyData.MoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌 대상이 IDamageable(데미지를 받을 수 있는지) 확인
        IDamageable damageable =
            other.GetComponent<IDamageable>();

        // IDeamageable을 가지고 있으면
        if (damageable != null)
        {
            //적 공격력만큼 데미지 전달
            damageable.TakeDamage(enemyData.AttackPower);
        }
    }

}
