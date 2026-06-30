using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPoolManager : SceneSingleton<EnemyPoolManager>
{
    [Header("Pool Setting")]
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    // EnemyData별 ObjectPool을 저장
    private readonly Dictionary<EnemyData, ObjectPool<Enemy>> pools
        = new Dictionary<EnemyData, ObjectPool<Enemy>>();

    public Enemy GetEnemy(EnemyData data)
    {
        if (!pools.TryGetValue(data, out ObjectPool<Enemy> pool))
        {
            pool = CreatePool(data);
            pools.Add(data, pool);
        }

        return pool.Get();
    }

    public void ReturnEnemy(Enemy enemy)
    {
        if (enemy == null)
        {
            return;
        }

        EnemyData data = enemy.EnemyData;

        if (pools.TryGetValue(data, out ObjectPool<Enemy> pool))
        {
            pool.Release(enemy);
        }
        else
        {
            Destroy(enemy.gameObject);
        }
    }

    private ObjectPool<Enemy> CreatePool(EnemyData data)
    {
        EnemyData currentData = data;

        return new ObjectPool<Enemy>(
            OnCreateEnemy,
            OnGetEnemy,
            OnReleaseEnemy,
            OnDestroyEnemy,
            true,
            defaultCapacity,
            maxSize
        );

        Enemy OnCreateEnemy()
        {
            Enemy enemy = Instantiate(currentData.EnemyPrefab).GetComponent<Enemy>();
            enemy.OnDead += ReturnEnemy;
            enemy.gameObject.SetActive(false);
            return enemy;
        }

        void OnGetEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        void OnReleaseEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        void OnDestroyEnemy(Enemy enemy)
        {
            enemy.OnDead -= ReturnEnemy;
            Destroy(enemy.gameObject);
        }
    }
}