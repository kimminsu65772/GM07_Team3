using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        TimeManagerTest.Instance.SubscribeToTimeChanged(UpdateTimerUI);
    }

    private void UpdateTimerUI(int remainingTime)
    {
        int minutes = remainingTime / 60;
        int seconds = remainingTime % 60;

        // D2는 한 자리 숫자도 두 자리로 표시하도록 포맷팅하는 것을 의미함.
        // ex) 5 -> 05
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    private void OnDisable()
    {
        if (TimeManagerTest.Instance == null) return;
        TimeManagerTest.Instance.UnsubscribeFromTimeChanged(UpdateTimerUI);
    }
}
