using System;
using UnityEngine;

public class TimeManagerTest : Singleton<TimeManagerTest>
{
    [Header("Max Time")]
    [SerializeField] private float maxTime = 1200f;

    public event Action<int> OnTimeChanged;

    private float elapsedTime = 0f;
    private int lastSecond = -1;


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
