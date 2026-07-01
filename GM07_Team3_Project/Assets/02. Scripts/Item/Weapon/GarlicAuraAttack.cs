using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class GarlicAuraAttack : MonoBehaviour
{
    [Header("오라 설정")]
    [SerializeField] private float radius = 3.0f;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private LayerMask targetLayer;

    [Header("위치 설정")]
    [SerializeField] private float heightOffset = -1.0f;

    private Transform owner;
    private float damage;
    private float timer;
    
    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    public void Init(float damage, Transform owner)
    {
        this.damage = damage;
        this.owner = owner;

        timer = 0.0f;

        if(targetLayer.value == 0)
        {
            targetLayer = LayerMask.GetMask("Target");
        }
        UpdatePosition();
    }

    private void Update()
    {
        if (owner == null)
        {
            Destroy(gameObject);
            return;
        }
        UpdatePosition();

        timer += Time.deltaTime;

        if(timer >= damageInterval)
        {
            timer = 0.0f;
            DamageEnemiesInRange();
        }

    }

    private void UpdatePosition()
    {
        transform.position = owner.position + Vector3.up * heightOffset;
    }

    private void DamageEnemiesInRange()
    {
        hitTargets.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayer);

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable == null)
            {
                continue;
            }

            if (hitTargets.Contains(damageable))
            {
                continue;
            }

            hitTargets.Add(damageable);
            damageable.TakeDamage(damage);
        }
    }
        private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}



