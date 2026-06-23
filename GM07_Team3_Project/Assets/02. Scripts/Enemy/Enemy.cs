using System;
using UnityEngine;
using UnityEngine.AI; // NavMesh 사용위해 필요

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] protected EnemyData enemyData;

    protected float currentHp;

    // 내비게이션 변수
    protected NavMeshAgent agent;
    protected Transform target;

    // 체력 변화, 사망 이벤트 옵저버 패턴
    public event Action<float, float> OnHpChanged; // 체력 변경 알림
    public event Action<Enemy> OnDead; // 사망 알림

    protected virtual void OnEnable()
    {
        currentHp = enemyData.MaxHp;

        OnHpChanged?.Invoke(currentHp, enemyData.MaxHp);

        // 컴포넌트, 타겟 세팅용
        agent = GetComponent<NavMeshAgent>();
        // 내비게이션 속도를 기존 속도 데이터와 동기화
        if (agent != null && enemyData != null)
        {
            agent.speed = enemyData.MoveSpeed;
        }

        FindPlayer();
    }

    // 자식 공통 플레이어 탐색기능
    private void FindPlayer()
    {
        if (target != null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    // 자식 공통 이동 기능
    protected virtual void MoveToTarget()
    {
        if (target == null) return;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
    }

    //피격 판정
    public virtual void TakeDamage(float damage)
    {
        float finalDamage =
            Mathf.Max(1f, damage - enemyData.DefensePower);

        currentHp -= finalDamage;

        currentHp = Mathf.Clamp(currentHp, 0f, enemyData.MaxHp);

        OnHpChanged?.Invoke(currentHp, enemyData.MaxHp);

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDead?.Invoke(this);
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }
}