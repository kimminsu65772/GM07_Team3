using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [Header("경험치 바 설정")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private Image expFillImage;

    [Header("색상 변환 설정")]
    // 비율에 따른 색깔 설정
    [SerializeField] private Gradient expGradient;

    // 무지개 색상 변환 속도 설정
    [SerializeField] private float rainbowSpeed = 1f;


    // Value에 따라 경험치 바의 색상을 변경하는 메서드
    private void Update()
    {
        UpdateExpBar();
    }


    private void UpdateExpBar()
    {
        float value = expSlider.value;

        if (value >= 0.999f)
        {
            // 무지개 색상 변환을 위한 값
            // Mathf.PingPong를 사용하여 시간에 따라 0과 1 사이를 왕복으로 변화시키는 값 생성
            float hue = Mathf.PingPong(Time.time * rainbowSpeed, 1f);

            // HSV 색상 공간에서 무지개 색상 생성
            expFillImage.color = Color.HSVToRGB(hue, 1f, 1f);
            return;
        }
        
        expFillImage.color = expGradient.Evaluate(value);
    }
}
