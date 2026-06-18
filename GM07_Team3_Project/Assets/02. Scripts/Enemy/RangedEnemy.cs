using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Target")]
    [SerializeField] private Transform target;

    private float attackTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        FindPlayer();
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        attackTime += Time.deltaTime;

        if (attackTime >= enemyData.AttackSpeed)
        {
            Shoot();
            attackTime = 0f;
        }
    }

    private void FindPlayer()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Shoot()
    {
        Debug.Log("원거리 공격");
    }
}
