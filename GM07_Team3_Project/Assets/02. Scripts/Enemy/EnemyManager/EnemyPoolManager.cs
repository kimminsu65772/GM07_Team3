using UnityEngine;
using UnityEngine.Pool;

public class EnemyPoolManager : Singleton<EnemyPoolManager>
{
    [Header("Melee Enemy")]
    [SerializeField] private MeleeEnemy meleeEnemyPrefab;

    [Header("Ranged Enemy")]
    [SerializeField] private RangedEnemy rangedEnemyPrefab;

    [Header("Pool Setting")]
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private ObjectPool<MeleeEnemy> meleePool;

    private ObjectPool<RangedEnemy> rangedPool;

    protected override void Awake()
    {
        base.Awake();

        InitializePools();
    }

    private void InitializePools()
    {
        meleePool = new ObjectPool<MeleeEnemy>
            (CreateMeleeEnemy, OnGetMeleeEnemy, OnReleaseMeleeEnemy,
            OnDestroyMeleeEnemy, true, defaultCapacity, maxSize);

        rangedPool = new ObjectPool<RangedEnemy>
            (CreateRangedEnemy, OnGetRangedEnemy, OnReleaseRangedEnemy,
            OnDestroyRangedEnemy, true, defaultCapacity, maxSize);
    }

    private MeleeEnemy CreateMeleeEnemy()
    {
        MeleeEnemy enemy = Instantiate(meleeEnemyPrefab);

        enemy.OnDead += ReturnMeleeEnemy;

        enemy.gameObject.SetActive(false);

        return enemy;
    }

    private void OnGetMeleeEnemy(MeleeEnemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseMeleeEnemy(MeleeEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyMeleeEnemy(MeleeEnemy enemy)
    {
        enemy.OnDead -= ReturnMeleeEnemy;

        Destroy(enemy.gameObject);
    }
    
    public MeleeEnemy GetMeleeEnemy()
    {
        return meleePool.Get();
    }

    public void ReturnMeleeEnemy(Enemy enemy)
    {
        if (enemy is MeleeEnemy meleeEnemy)
        {
            meleePool.Release(meleeEnemy);
        }
    }

    private RangedEnemy CreateRangedEnemy()
    {
        RangedEnemy enemy =
            Instantiate(rangedEnemyPrefab);

        enemy.OnDead += ReturnRangedEnemy;

        enemy.gameObject.SetActive(false);

        return enemy;
    }

    private void OnGetRangedEnemy(RangedEnemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseRangedEnemy(RangedEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyRangedEnemy(RangedEnemy enemy)
    {
        enemy.OnDead -= ReturnRangedEnemy;

        Destroy(enemy.gameObject);
    }

    public RangedEnemy GetRangedEnemy()
    {
        return rangedPool.Get();
    }

    public void ReturnRangedEnemy(Enemy enemy)
    {
        if (enemy is RangedEnemy rangedEnemy)
        {
            rangedPool.Release(rangedEnemy);
        }
    }
    
}