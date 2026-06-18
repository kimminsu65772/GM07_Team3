using UnityEngine;
using UnityEngine.Pool;

public class AttackObject : MonoBehaviour
{
    protected float damage;
    protected Vector3 direction;
    [SerializeField] private float lifeTime = 3.0f;


    private Rigidbody rb;
    private Collider col;

    //투사체 프리펩이 돌아갈 풀
    private IObjectPool<GameObject> pool;

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }


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


    protected virtual void Return()
    {
        if(pool != null)
        {
            //풀이 있으면 반납
            pool.Release(gameObject);
        }
        else
        {
            //없으면 삭제
            Destroy(gameObject);
        }
    }

}
