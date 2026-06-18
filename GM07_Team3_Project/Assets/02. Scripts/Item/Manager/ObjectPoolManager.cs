using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [Header("풀링할 공격 프리펩")]
    [SerializeField] private GameObject attackPrefab;

    [Header("풀링 설정")]
    [SerializeField] private int defaultCapicity = 10;
    [SerializeField] private int maxPoolSize = 30;

    // 실제 풀 보관소 attackPool
    private IObjectPool<GameObject> attackPool;

    private void Awake()
    {
        


    }

    private void InitPool()
    {
      //  attackPool = new ObjectPool<GameObject>()
    }

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

    private void OnTakeFromPool(GameObject poolObj)
    {
        poolObj.SetActive(true);
    }
    private void OnReturnedtoPool(GameObject poolObj)
    {
        poolObj.SetActive(false);
    }
    private void OnDestroyPoolOnject(GameObject poolObj)
    {
        Destroy(poolObj);
    }
    public GameObject GetAttackObject()
    {
        return attackPool.Get();
    }




    

}
