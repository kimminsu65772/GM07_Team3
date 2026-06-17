using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

/*
 * MenuUI
 * MenuUI는 메뉴 버튼 UI에 애니메이션 효과를 적용하는 스크립트입니다.
 * 마우스와의 상호작용과 버튼 클릭 시 애니메이션 효과를 어떤 방식으로 적용할 것인지 정의합니다.
 */
public class MenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // 애니메이션 타겟이 되는 버튼의 텍스트
    [SerializeField] private TextMeshProUGUI buttonText; 

    // 버튼에 마우스가 올라갔을 때의 스케일을 정의하는 변수
    [Header("Hover Settings")]
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float pressScale = 0.9f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Color originalTextColor;
    private bool isClickable;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.transform.localScale;
        if (buttonText != null)
        {
            originalTextColor = buttonText.color;
        }
        isClickable = true;
    }

    // 버튼 위에 마우스가 올라갔을 때 호출되는 메서드
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClickable) return;
        if (!buttonText) return;

        // 현재 진행중인 애니메이션이 있다면 중지 
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, 0.2f).SetEase(Ease.OutBack);
        // 텍스트 색상을 변경
        buttonText.DOColor(Color.green, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClickable) return;
        if (!buttonText) return;

        transform.DOKill();
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack);
        buttonText.DOColor(originalTextColor, 0.2f).SetEase(Ease.OutBack);
    }

    // 마우스 버튼을 누르는 동안 호출되는 메서드
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isClickable) return;
        if (!buttonText) return;
        transform.DOKill();
        transform.DOScale(originalScale * pressScale, 0.1f).SetEase(Ease.OutBack);
        buttonText.DOFade(0.5f, 0.1f).SetEase(Ease.OutBack);
    }

    // 마우스 버튼에서 손을 뗄 때 호출되는 메서드
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isClickable) return;
        if (!buttonText) return;
        transform.DOKill();
        transform.DOScale(originalScale, 0.1f).SetEase(Ease.OutBack);
        buttonText.DOFade(1f, 0.1f).SetEase(Ease.OutBack);
    }
}
