using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Vector3 moveDirection;
    private float lifeTimer;

    private void OnEnable()
    {
        lifeTimer = lifeTime;
        moveDirection = Vector3.zero;
    }

    private void Update()
    {
        transform.position +=
            moveDirection * speed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            EnemyBulletPool.Instance.Return(this);
        }
    }

    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player"))
        {
            return;
        }

        IDamageable damageable =
            other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(10f);
        }

        EnemyBulletPool.Instance.Return(this);
    }
}
