using UnityEngine;

/*
 * SceneSingleton
 * SceneSingleton 클래스는 특정 씬에서만 존재해야 하는 싱글톤 객체를 구현하기 위한 제네릭 클래스입니다.
 * 일반 싱글톤 구조를 적용하면 씬을 이동하는 과정에서 설정이 초기화되는 문제가 발생할 수 있으므로,
 * DontDestroyOnLoad를 사용하지 않고 씬 전환 시 자동으로 파괴되는 구조로 구현했습니다.
 */
public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private bool isDuplicateInstance;

    public static bool HasInstance => instance != null;

    protected bool IsDuplicateInstance => isDuplicateInstance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            return;
        }

        if (instance != this)
        {
            isDuplicateInstance = true;
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
