using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool Instance; // 싱글톤

    [SerializeField] private int poolSize = 30;

    [SerializeField] private EnemyBullet bulletPrefab;

    // 몬스터 총알 선입선출
    private Dictionary<EnemyBullet, Queue<EnemyBullet>> pools =
        new Dictionary<EnemyBullet, Queue<EnemyBullet>>();

    private void Awake()
    {
        Instance = this;
    }

    public EnemyBullet Get(EnemyBullet bulletPrefab)
    {
        if (!pools.TryGetValue(bulletPrefab, out Queue<EnemyBullet> pool))
        {
            pool = new Queue<EnemyBullet>();

            for (int i = 0; i < poolSize; i++)
            {
                EnemyBullet bullet = Instantiate(bulletPrefab);
                bullet.gameObject.SetActive(false);
                pool.Enqueue(bullet);
            }

            pools.Add(bulletPrefab, pool);
        }
        EnemyBullet result;

        if (pool.Count > 0)
        {
            result = pool.Dequeue();
        }
        else
        {
            // 부족할때 임시생성
            result = Instantiate(bulletPrefab);
        }

        result.gameObject.SetActive(true);
        return result;
    }

    public void Return(EnemyBullet bullet, EnemyBullet bulletPrefab)
    {
        bullet.gameObject.SetActive(false);

        if (!pools.TryGetValue(bulletPrefab, out Queue<EnemyBullet> pool))
        {
            pool = new Queue<EnemyBullet>();
            pools.Add(bulletPrefab, pool);
        }
        
        pool.Enqueue(bullet);
    }
}
