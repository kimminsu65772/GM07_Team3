using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public float maxHp = 100;
    private float currentHp;

    void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        if (currentHp <= 0f) return;

        currentHp -= damage;

        if (currentHp <= 0f)
        {
            Die();
        }

        void Die()
        {
            gameObject.SetActive(false);
        }
    }
}
