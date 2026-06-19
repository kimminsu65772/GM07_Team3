using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    //싱글톤으로 구성
    public static ObjectPoolManager Instance {  get; private set; }

    [Header("풀링할 공격 프리펩")]
    [SerializeField] private GameObject attackPrefab;

    [Header("풀링 설정")]
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 30;

    // 실제 풀 보관소 attackPool
    private IObjectPool<GameObject> attackPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        InitPool();
    }

    private void InitPool()
    {
        attackPool = new ObjectPool<GameObject>(
            CreatePooledItem,//만들고 
            OnTakeFromPool, //꺼내고
            OnReturnedtoPool, //반납하고
            OnDestroyPoolOnject, //삭제
            true,
            defaultCapacity,
            maxPoolSize);
    }

    //오브젝트 생성 
    private GameObject CreatePooledItem()
    {
        GameObject PoolObj = Instantiate(attackPrefab);
        PoolObj.SetActive(true);

        AttackObject attackObject = PoolObj.GetComponent<AttackObject>();

        if(attackObject != null )
        {
            attackObject.SetPool(attackPool);
        }
        return PoolObj;
    }

    //풀에서 꺼내오기
    private void OnTakeFromPool(GameObject poolObj)
    {
        poolObj.SetActive(true);
    }

    //풀로 반납
    private void OnReturnedtoPool(GameObject poolObj)
    {
        poolObj.SetActive(false);
    }

    //삭제
    private void OnDestroyPoolOnject(GameObject poolObj)
    {
        Destroy(poolObj);
    }

    //외부에서 오브젝트 꺼낼때
    public GameObject GetAttackObject()
    {
        return attackPool.Get();
    }




    

}
