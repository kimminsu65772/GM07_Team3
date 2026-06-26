using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [Header("체력 바 설정")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image hpFillImage;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("색상 변환 설정")]
    // 비율에 따른 색깔 설정
    [SerializeField] private Gradient hpGradient;



    // Value에 따라 경험치 바의 색상을 변경하는 메서드
    private void Update()
    {
        UpdateHPBar();
    }


    private void UpdateHPBar()
    {
        float value = hpSlider.value;
        hpFillImage.color = hpGradient.Evaluate(value);
    }

    private void UpdateHPText(float currentHp, float maxHp)
    {
        hpText.text = $"{currentHp}/{maxHp}";
    }

    public void SetHPBar(float currentHp, float maxHp)
    {
        hpSlider.value = currentHp / maxHp;
        UpdateHPText(currentHp, maxHp);
    }
}
