using System.Collections.Generic;
using UnityEngine;
public class MeleeEffectAttack : AttackObject
{
    [Header("근접 공격 판정")]
    [SerializeField] private float radius = 2.0f;
    [SerializeField] private float attackAngle = 180.0f;
   // [SerializeField] private LayerMask targetLayer; private HashSet<IDamageable>

    //hitTargets = new HashSet<IDamageable>();

    public override void Init(float damage, Vector3 direction)
    {
       // base.Init(damage, direction); hitTargets.Clear();
        transform.rotation = Quaternion.LookRotation(direction);
        //CheckMeleeHit();
    }
    //private void CheckMeleeHit()
    //{

        //Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayer);
        //{
        //    foreach (Collider hit in hits)
        //    {
        //        Vector3 toTarget =
        //        hit.transform.position - transform.position; toTarget.y = 0.0f;

        //        Vector3 attackDirection = direction; attackDirection.y = 0.0f;

        //        float currentAngle = Vector3.Angle(attackDirection, toTarget);

        //        if (currentAngle > attackAngle * 0.5f) { continue; }

        //       // IDamageable damageable = hit.GetComponentInParent<IDamageable>();

        //        //if (damageable == null) { continue; }
        //        //if (hitTargets.Contains(damageable)) { continue; }

        //       // hitTargets.Add(damageable); damageable.TakeDamage(damage);
        //    }



        private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, radius);
    }
}
