using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [Header("경험치 바 설정")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private Image expFillImage;
    [SerializeField] private TextMeshProUGUI expText;

    [Header("색상 변환 설정")]
    // 비율에 따른 색깔 설정
    [SerializeField] private Gradient expGradient;

    // 무지개 색상 변환 속도 설정
    [SerializeField] private float rainbowSpeed = 1f;
    [SerializeField] private float levelUpRainbowDuration = 3f;

    private float rainbowTimer;
    // 레벨업 애니메이션이 끝난 후 현재 경험치 바의 값으로 돌아가기 위한 변수
    private float targetValue;

    // Value에 따라 경험치 바의 색상을 변경하는 메서드
    private void Update()
    {
        UpdateExpBar();
    }


    private void UpdateExpBar()
    {
        float value = expSlider.value;

        if (rainbowTimer > 0f)
        {
            rainbowTimer -= Time.unscaledDeltaTime;
            expSlider.value = 1f;
            // 무지개 색상 변환을 위한 값
            // Mathf.PingPong를 사용하여 시간에 따라 0과 1 사이를 왕복으로 변화시키는 값 생성
            float hue = Mathf.PingPong(Time.unscaledTime * rainbowSpeed, 1f);

            // HSV 색상 공간에서 무지개 색상 생성
            expFillImage.color = Color.HSVToRGB(hue, 1f, 1f);
            return;
        }
        if (!Mathf.Approximately(expSlider.value, targetValue))
        {
            expSlider.value = targetValue;
            value = targetValue;
        }

        expFillImage.color = expGradient.Evaluate(value);
    }

    public void SetExpBar(float currentExp, float maxExp)
    {
        if (maxExp <= 0f)
        {
            targetValue = 1f;

            if (rainbowTimer <= 0f)
            {
                expSlider.value = targetValue;
            }

            return;
        }

        targetValue = Mathf.Clamp01(currentExp / maxExp);

        if (rainbowTimer <= 0f)
        {
            expSlider.value = targetValue;
        }
    }

    public void ChangeLevelText(int currentLevel)
    {
        expText.text = $"Lv. {currentLevel}";
    }

    public void PlayLevelUpEffect()
    {
        rainbowTimer = levelUpRainbowDuration;
        expSlider.value = 1f;
    }
}
