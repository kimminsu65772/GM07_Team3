using System;
using UnityEngine;

public class TimeManagerTest : MonoBehaviour
{
    [Header("Max Time")]
    [SerializeField] private float maxTime = 1200f;

    public event Action<int> OnTimeChanged;

    private float elapsedTime = 0f;
    private int lastSecond = -1;

    // Time 매니저의 경우 다른 매니저와 다르게 게임 씬에서만 존재해야 하므로 싱글톤 패턴을 적용하되, DontDestroyOnLoad를 사용하지 않고 씬이 변경될 때마다 새로 생성되도록 한다. (임시)
    private static TimeManagerTest instance;
    public static bool HasInstance => instance != null;
    public static TimeManagerTest Instance
    {
        get
        {
            if (instance == null)
            {
                // 먼저 씬에서 매니저 객체를 찾아 인스턴스에 할당을 먼저 시도
                instance = FindFirstObjectByType<TimeManagerTest>();

                // 씬에 매니저 객체가 없는 경우, 매니저 타입과 동일한 이름의 새로운 게임 오브젝트를 생성
                // 그 후 새로 생성된 게임 오브젝트에 매니저 컴포넌트를 추가하고 이를 인스턴스에 할당
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(TimeManagerTest).Name);
                    instance = obj.AddComponent<TimeManagerTest>();
                }
            }

            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        UIManager.Instance.onPausePressed -= ToggleTimeScale;
        UIManager.Instance.onPausePressed += ToggleTimeScale;
    }

    private void OnDisable()
    {
        if (!UIManager.HasInstance) return;
        UIManager.Instance.onPausePressed -= ToggleTimeScale;

        if (instance == this)
        {
            instance = null;
        }
        // 일시정지 상태에서 메인 화면으로 넘어가버리면
        // Time.timeScale이 0인 상태로 남아있어서
        // 메인 화면에서 아무것도 작동하지 않는 문제가 발생할 수 있으므로,
        // TimeManagerTest가 파괴될 때 Time.timeScale을 1로 초기화하여 일시정지 상태가 유지되지 않도록 한다.
        Time.timeScale = 1f;
    }


    void Update()
    {
        UpdateTimeChanged();
    }

    // 게임을 일시정지하거나 재개하는 기능을 수행하는 메서드
    public void ToggleTimeScale()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }

    // 초 단위로 시간 변경 이벤트를 발생시키는 메서드
    private void UpdateTimeChanged()
    {
        elapsedTime += Time.deltaTime;

        // 경과 시간이 최대 시간을 초과하면 더이상 이벤트를 발생시키지 않음.
        if (elapsedTime > maxTime) return;

        int remainingTime = Mathf.FloorToInt(maxTime - elapsedTime);

        // 남은 시간을 초 단위로 계산하여 이전 초와 비교하여 변경이 있는지 확인한다.
        if (remainingTime == lastSecond) return;

        lastSecond = remainingTime;
        OnTimeChanged?.Invoke(remainingTime);
    }

    // 다른 클래스에서 이벤트를 구독하는 메서드
    public void SubscribeToTimeChanged(Action<int> callback)
    {
        OnTimeChanged += callback;
    }

    public void UnsubscribeFromTimeChanged(Action<int> callback)
    {
        OnTimeChanged -= callback;
    }
}
