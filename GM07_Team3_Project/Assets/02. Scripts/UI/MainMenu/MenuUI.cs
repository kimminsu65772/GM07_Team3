using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using System;

/*
 * MenuUI
 * MenuUI는 메뉴 버튼 UI에 애니메이션 효과를 적용하는 스크립트입니다.
 * 마우스와의 상호작용과 버튼 클릭 시 애니메이션 효과를 어떤 방식으로 적용할 것인지 정의합니다.
 */
public class MenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // 애니메이션 타겟이 되는 버튼의 텍스트
    [Header("Button Text")]
    [SerializeField] private TextMeshProUGUI buttonText;

    // 버튼에 마우스가 올라갔을 때의 스케일을 정의하는 변수
    [Header("Animation Scale Setting")]
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float pressScale = 0.9f;

    [Header("Start YOffset Seting")]
    [SerializeField] private float startYOffset = -80f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector3 originalScale;
    private Color originalTextColor;
    private bool IsClickable;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.transform.localScale;
        originalPosition = rectTransform.anchoredPosition;
        if (buttonText != null)
        {
            originalTextColor = buttonText.color;
        }
    }

    // 버튼 위에 마우스가 올라갔을 때 호출되는 메서드
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsClickable) return;
        if (!buttonText) return;

        // 현재 진행중인 애니메이션이 있다면 중지
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, 0.2f).SetEase(Ease.OutBack);
        // 텍스트 색상을 변경
        buttonText.DOColor(Color.green, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsClickable) return;
        if (!buttonText) return;

        transform.DOKill();
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack);
        buttonText.DOColor(originalTextColor, 0.2f).SetEase(Ease.OutBack);
    }

    // 마우스 버튼을 누르는 동안 호출되는 메서드
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsClickable) return;
        if (!buttonText) return;
        transform.DOKill();
        transform.DOScale(originalScale * pressScale, 0.1f).SetEase(Ease.OutBack);
        buttonText.DOFade(0.5f, 0.1f).SetEase(Ease.OutBack);
    }

    // 마우스 버튼에서 손을 뗄 때 호출되는 메서드
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsClickable) return;
        if (!buttonText) return;
        transform.DOKill();
        transform.DOScale(originalScale, 0.1f).SetEase(Ease.OutBack);
        buttonText.DOFade(1f, 0.1f).SetEase(Ease.OutBack);
    }

    public void ActiveMenuButton()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(buttonText.DOFade(1f, 0.25f));
        sequence.Join(rectTransform.DOAnchorPosY(originalPosition.y, 0.25f).SetEase(Ease.OutCubic));
        sequence.Join(transform.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack));
    }

    public void DeactiveMenuButton()
    {
        IsClickable = false;
        transform.DOKill();
        rectTransform.DOKill();
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y + startYOffset);
        transform.localScale = Vector3.zero;
    }

    // 시작 애니메이션 재생 후 버튼이 클릭 가능한 상태로 전환하기 위해 따로 Setter 구현
    public void SetIsClickable(bool value)
    {
        IsClickable = value;
    }

    public void PlaySelectTween()
    {
        transform.DOKill();
        rectTransform.DOKill();
        buttonText.DOKill();
        transform.DOScale(originalScale, 0.3f).SetEase(Ease.InBack);
    }
}
