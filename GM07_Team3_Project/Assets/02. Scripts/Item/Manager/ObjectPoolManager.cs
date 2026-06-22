using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    //싱글톤으로 구성
    public static ObjectPoolManager Instance {  get; private set; }

    //[Header("풀링할 공격 프리펩")]
    //[SerializeField] private GameObject attackPrefab;

    [Header("풀링 설정")]
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 30;

    // Dictionary로 풀 만들기
    private Dictionary<GameObject, IObjectPool<GameObject>> pools =
        new Dictionary<GameObject, IObjectPool<GameObject>>();

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
    }


    //오브젝트 생성 
    private GameObject CreatePooledItem(GameObject prefab, IObjectPool<GameObject> pool)
    {
        GameObject PoolObj = Instantiate(prefab);
        PoolObj.SetActive(false);

        AttackObject attackObject = PoolObj.GetComponent<AttackObject>();

        if(attackObject != null )
        {
            attackObject.SetPool(pool);
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
    private void OnDestroyPoolObject(GameObject poolObj)
    {
        Destroy(poolObj);
    }

    //외부에서 오브젝트 꺼낼때
    public GameObject GetAttackObject(GameObject prefab)
    {
        if(prefab == null) return null;

        if (pools.ContainsKey(prefab) == false)
        {
            CreatePool(prefab);
        }

        //pools 딕셔너리에서 prefab 에 해당하는 pool을 찾고
        // 그 pool에서 오브젝트를 꺼냄
            return pools[prefab].Get();
    }

    //이 prefab만 담당하는 ObjecPool을 하나 만들어서
    // 그 Pool을 Dictionary에  저장
    private void CreatePool(GameObject prefab)
    {
        IObjectPool<GameObject> pool = null;

        pool = new ObjectPool<GameObject>(
            () => CreatePooledItem(prefab, pool),
            OnTakeFromPool,
            OnReturnedtoPool,
            OnDestroyPoolObject,
            true,
            defaultCapacity,
            maxPoolSize);

        pools.Add(prefab, pool);
    }




    

}
