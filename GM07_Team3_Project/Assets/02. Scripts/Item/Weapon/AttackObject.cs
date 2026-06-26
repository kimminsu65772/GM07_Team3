using UnityEngine;
using UnityEngine.Pool;

public class AttackObject : MonoBehaviour
{
    protected float damage;
    protected Vector3 direction;

    [SerializeField] private float lifeTime = 3.0f;

    private Rigidbody rb;
    private Collider col;

    // 투사체 프리펩이 돌아갈 풀
    private IObjectPool<GameObject> pool;

    // 이미 풀에 반납됐는지 확인
    private bool isReturned;

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public virtual void Init(float damage, Vector3 direction)
    {
        this.damage = damage;
        this.direction = direction.normalized;

        // 풀에서 다시 꺼내졌으니 반납 상태 초기화
        isReturned = false;

        // 이전 Invoke 삭제
        CancelInvoke();

        // 생명 주기 끝나면 풀로 반납
        Invoke(nameof(Return), lifeTime);
    }

    protected virtual void Return()
    {
        // 이미 반납된 오브젝트면 다시 반납하지 않음
        if (isReturned)
        {
            return;
        }

        isReturned = true;

        // lifeTime Invoke 중복 방지
        CancelInvoke();

        if (pool != null)
        {
            // 풀이 있으면 반납
            pool.Release(gameObject);
        }
        else
        {
            // 없으면 삭제
            Destroy(gameObject);
        }
    }
}