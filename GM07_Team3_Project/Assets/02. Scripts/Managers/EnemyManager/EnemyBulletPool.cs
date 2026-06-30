using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool Instance; // 싱글톤

    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private int poolSize = 30;

    // 몬스터 총알 선입선출
    private Queue<EnemyBullet> pool = new Queue<EnemyBullet>();

    private void Awake()
    {
        Instance = this;

        for (int i =0; i < poolSize; i++)
        {
            EnemyBullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            pool.Enqueue(bullet);
        }
    }

    public EnemyBullet Get()
    {
        EnemyBullet bullet;

        if (pool.Count > 0)
        {
            bullet = pool.Dequeue();
        }

        else
        {
            // 부족할때 임시생성
            bullet = Instantiate(bulletPrefab);
        }

        bullet.gameObject.SetActive(true);
        return bullet;
    }

    public void Return(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        pool.Enqueue(bullet);
    }
}
