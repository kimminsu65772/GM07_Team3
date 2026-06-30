using UnityEngine;

/*
 * SceneSingleton
 * Use this for managers that should behave like a singleton only inside the
 * current scene. Unlike Singleton<T>, this class does not use DontDestroyOnLoad.
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
