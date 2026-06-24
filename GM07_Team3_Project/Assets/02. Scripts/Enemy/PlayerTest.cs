using UnityEngine;
using UnityEngine.AI; // NavMesh 사용위해 필요

public class PlayerTest : MonoBehaviour, IDamageable
{
    public float maxHp = 100;
    [SerializeField] private float currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        if (currentHp <= 0f) return;

        currentHp -= damage;
        Debug.Log($"플레이어 피격. 남은 체력: {currentHp}");

        if (currentHp <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("사망");
        gameObject.SetActive(false);
    }
    
}
