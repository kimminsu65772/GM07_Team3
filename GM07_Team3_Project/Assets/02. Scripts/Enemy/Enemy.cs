using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] protected EnemyData enemyData;

    protected float currentHp;

    // 체력 변화, 사망 이벤트 옵저버 패턴
    public event Action<float, float> OnHpChanged; // 체력 변경 알림
    public event Action<Enemy> OnDead; // 사망 알림

    protected virtual void OnEnable()
    {
        currentHp = enemyData.MaxHp;

        OnHpChanged?.Invoke(currentHp, enemyData.MaxHp);
    }

    public virtual void TakeDamage(float damage)
    {
        float finalDamage =
            Mathf.Max( 1f, damage - enemyData.DefensePower);

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