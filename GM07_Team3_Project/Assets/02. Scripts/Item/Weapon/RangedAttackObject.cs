using UnityEngine;

public class RangedAttackObject : AttackObject
{
    [Header("투사체 속도")]
    [SerializeField] private float speed = 10.0f;

    [Header("공격 대상 레이어")]
    [SerializeField] private LayerMask targetLayer;


    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    //타겟 콜라이더랑 만났을때 실행
    private void OnTriggerEnter(Collider other)
    {
        //적 레이어가 아니면 종료
        if (IsTargetLayer(other.gameObject.layer) == false) { return; }
        //IDamageable 가져오기
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        //없으면 넘겨
        if (damageable == null) { return; }
        //실제 데미지 주기
        damageable.TakeDamage(damage);
        //데미지 줬으면 다시 풀링으로 리턴
        Return();
    }

    //이 오부젝트의 레이어가 targetLasyer에 포함되어 있는지 검사
    private bool IsTargetLayer(int objectLayer)
    {
        return (targetLayer.value & (1 << objectLayer)) != 0;
    }
}

