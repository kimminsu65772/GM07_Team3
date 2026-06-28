using System.Collections.Generic;
using UnityEngine;

public class GarlicAuraAttack : MonoBehaviour
{
    [Header("오라 설정")]
    //공격 범위설정
    [SerializeField] private float radius = 3.0f;
    //공속
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private LayerMask targetLayer;

    //[Header("시각 효과")]
    ////근접공격과 동일하게 파티클에서 시각적 효과 넣기
    //[SerializeField] private Transform visualRoot;

    private Transform owner;
    private float damage;

    private float timer;

    //적 중복데미지 방지용 해쉬셋
    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    public void Init(float damage, Transform owner)
    {
        this.damage = damage;
        this.owner = owner;

        timer = 0.0f;

        UpdatePosition();
        //UpdateVisualScale();
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

        if (timer >= damageInterval)
        {
            timer = 0.0f;
            DamageEnemiesInRange();
        }
    }

    //오라를 캐릭터에 고정
    private void UpdatePosition()
    {
        transform.position = owner.position + Vector3.up;
    }

    //데미지 함수
    private void DamageEnemiesInRange()
    {
        //데미지 맞았으면 초기화 
        hitTargets.Clear();

        //그접공격과 동일한 검사방식
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayer);

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponentInParent<IDamageable>();
            //데미지 받을 수 없으면 넘기고
            if (damageable == null)
            {
                continue;
            }
            //이미 맞은 적도 넘겨
            if (hitTargets.Contains(damageable))
            {
                continue;
            }
            //데미지 주기
            hitTargets.Add(damageable);
            damageable.TakeDamage(damage);
        }
    }

    //눈에 보이는 원 크기를 공격 범위로 설정
    private void UpdateVisualScale()
    {
        //if(visualRoot == null)
        //{ return; }
        //if (visualRoot == null)
        //{
        //    return;
        //}
        ////반지름의 * 2 반큼 계산해서 원을 만들어 줌
        //visualRoot.localScale = new Vector3(radius * 2.0f, 1.0f, radius * 2.0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}