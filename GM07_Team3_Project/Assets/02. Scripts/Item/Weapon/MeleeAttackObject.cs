using UnityEngine;

public class MeleeAttackObject : AttackObject
{
    [SerializeField] private float radius = 2.0f; //공격 거리
    [SerializeField] private float attackAngle = 180.0f; // 각도
    [SerializeField] private LayerMask targetLayer;

    public override void Init(float damage, Vector3 direction)
    {
        base.Init(damage, direction);

        CheckMeleeHit();
    }

    private void CheckMeleeHit()
    {
        //내 위치 중심으로 radius 만큼의 구범위 안에 있는 cllider 들을 전부 찾는다
        Collider[] hits = Physics.OverlapSphere(transform.position, radius,targetLayer);

        //찾은 적 후보 하나씩 검사
        foreach(Collider hit in hits)
        {
            //적 방향 검사
            Vector3 toTarget = hit.transform.position - transform.position;
            //각도 검사
            float currentAngle = Vector3.Angle(direction,toTarget);
            //반원 안에 있는지 검사
            if(currentAngle <= attackAngle * 0.5f) 
            {
                //추가 예정
                //if(hit.TryGetComponent<IDamageble damageble>))
                //{
                //    dmaageble.TakeDamage(damage);
                //}
                
            }

        }
    }
}
