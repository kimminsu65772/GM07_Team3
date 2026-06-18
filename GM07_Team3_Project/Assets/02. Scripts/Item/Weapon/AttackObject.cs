using UnityEngine;

public class AttackObject : MonoBehaviour
{
    protected float damage;
    protected Vector3 direction;
    [SerializeField] private float lifeTime;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public virtual void Init(float  damage, Vector3 direction)
    {
        this.damage = damage;
        this.direction = direction.normalized;

        //이전 Invoke 삭제
        CancelInvoke();
        //생명 주기 끝나면 삭제
        Invoke(nameof(Return), lifeTime);

    }

    //IDamageable 인터페이스 형식 데미지

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
    //    {
    //        damageable.TakeDamage(damage);
    //        Return();
    //    }
    //}


    //일단 리턴으로 사용 이후 오브젝트풀링으로 관리
    protected virtual void Return()
    {
        Destroy(gameObject);
    }

}
