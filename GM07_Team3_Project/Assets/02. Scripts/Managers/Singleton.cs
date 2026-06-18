using UnityEngine;
using UnityEngine.Rendering;

/*
 * Singleton
 * Singleton 클래스는 모든 씬에서 각각의 매니저 객체가 하나씩만 존재할 수 있도록 보장하는 패턴입니다.
 * 객체가 이미 존재하는 경우, 새로운 객체는 파괴되고 기존 객체가 유지될 수 있도록 합니다.
 */
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 실제 싱글톤 인스턴스를 저장하는 정적 변수
    private static T instance;

    // 다른 스크립트에서 매니저 인스턴스에 접근할 수 있게 하는 정적 프로퍼티 
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 먼저 씬에서 매니저 객체를 찾아 인스턴스에 할당을 먼저 시도
                instance = FindFirstObjectByType<T>();

                // 씬에 매니저 객체가 없는 경우, 매니저 타입과 동일한 이름의 새로운 게임 오브젝트를 생성
                // 그 후 새로 생성된 게임 오브젝트에 매니저 컴포넌트를 추가하고 이를 인스턴스에 할당
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
        // 매니저 인스턴스가 없는 경우에는 현재 객체를 싱글톤 인스턴스로 설정
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 매니저 인스턴스가 존재하여 현재 객체랑 다른 경우에는 새로 생성한 현재 객체를 파괴함
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
