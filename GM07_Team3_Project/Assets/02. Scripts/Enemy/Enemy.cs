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

    // 플레이어 경험치 전달용
    private PlayerStatController playerStatController;


    // 체력 변화, 사망 이벤트 옵저버 패턴
    public event Action<float, float> OnHpChanged; // 체력 변경 알림
    public event Action<Enemy> OnDead; // 사망 알림

    protected virtual void OnEnable()
    {
        currentHp = enemyData.MaxHp;

        OnHpChanged?.Invoke(currentHp, enemyData.MaxHp);

        // 컴포넌트, 타겟 세팅용 NavMeshAgent 가져오기
        agent = GetComponent<NavMeshAgent>();

        // 내비게이션 속도를 기존 속도 데이터와 동기화
        if (agent != null && enemyData != null)
        {
            agent.speed = enemyData.MoveSpeed;
        }

        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateUpAxis = false;
            agent.updateRotation = true;
        }
    }

    public void Initialize(Transform player, PlayerStatController statController)
    {
        target = player;
        playerStatController = statController;
    }
        

    // 적이 스폰될 때 NavMesh가 있는 위치로 이동시키는 위치 보정 메서드
    public bool WarpToNavMesh(Vector3 position)
    {
        // 위치를 스포너가 지정한 위치로 이동시키되 NavMeshAgent가 없으므로 false 반환
        // 이 경우 플레이어를 추적할 수 없는 상태이므로 pool로 반환될 수 있게 false 반환
        if (agent == null)
        {
            transform.position = position;
            return false;
        }

        // NavMeshAgent가 있는 경우 지정한 위치로 이동시키고 결과를 isWarped에 저장
        bool isWarped = agent.Warp(position);

        // 이동이 불가능해 위치 보정에 실패한 경우 pool에 반환될 수 있게 false 반환
        if (!isWarped)
        {
            return false;
        }

        // pool에서 재사용 하는 과정에서 기존에 남아있을 수 있는 경로를 초기화하고 이동을 재개
        agent.ResetPath();
        agent.isStopped = false;

        return true;
    }

    protected virtual void MoveToTarget()
    {
        if (target == null)
        {
            return;
        }

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
        // 경험치 지급용
        if (playerStatController != null)
        {
            playerStatController.AddExperience
                (enemyData.Experience);
        }

        // 사망 이벤트 호출용
        OnDead?.Invoke(this);
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }
}
