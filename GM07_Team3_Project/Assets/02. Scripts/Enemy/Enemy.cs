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

    // 애니메이터 참조 변수
    private Animator enemyAnim;

    protected virtual void OnEnable()
    {
        currentHp = enemyData.MaxHp;

        OnHpChanged?.Invoke(currentHp, enemyData.MaxHp);

        // 컴포넌트, 타겟 세팅용 NavMeshAgent 가져오기
        agent = GetComponent<NavMeshAgent>();

        // 애니메이터 가져오기
        enemyAnim = GetComponentInChildren<Animator>();


        if (agent != null)
        {
            // NavMeshAgent가 캐릭터의 "위쪽 방향(Y축 Up)"까지 자동으로 보정하는 기능을 끔
            // (경사/회전/축 꼬임 때문에 모델이 기울어지는 현상을 방지할 때 사용)
            agent.updateUpAxis = false;

            agent.isStopped = false;
        }

        // 내비게이션 속도를 기존 속도 데이터와 동기화
        if (agent != null && enemyData != null)
        {
            agent.speed = enemyData.MoveSpeed;
        }
    }

    public void Initialize(Transform player, PlayerStatController statController)
    {
        target = player;
        playerStatController = statController;
    }
        

    // 플레이어 추적 기능
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

        // 사망시 제자리에 멈추고 더이상 추적 X
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        // 사망 애니 트리거
        if (enemyAnim != null)
        {
            enemyAnim.SetTrigger("Die");
        }

        // 사망 이벤트 호출용
        OnDead?.Invoke(this);
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }
}