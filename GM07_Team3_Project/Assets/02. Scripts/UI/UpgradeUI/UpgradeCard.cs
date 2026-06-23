using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UpgradeCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UpgradeData upgradeData;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // 버튼에 마우스가 올라갔을 때의 스케일을 정의하는 변수
    [Header("Animation Scale Setting")]
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float pressScale = 0.9f;

    [Header("Start YOffset Seting")]
    [SerializeField] private float startYOffset = -80f;

    [Header("Start Y Rotation Setting")]
    [SerializeField] private float startYRotation = 180f;

    public Action<UpgradeData> OnCardSelected;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Quaternion originalRotation;
    private Vector2 originalScale;

    private bool isClickable;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        originalRotation = rectTransform.rotation;
        originalScale = rectTransform.localScale;
    }

    private void OnEnable()
    {
        // 카드가 활성화될 때 초기 상태로 설정
        Initialize();
        isClickable = true;
        InitUI(upgradeData);
    }

    private void OnDisable()
    {
        Initialize();
        isClickable = false;
        upgradeData = null;
    }

    // 카드의 상태를 초기화하는 메서드
    private void Initialize()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        rectTransform.DOKill();
        rectTransform.anchoredPosition = originalPosition + new Vector2(0f, startYOffset);
        rectTransform.rotation = Quaternion.Euler(0f, startYRotation, 0f);
        rectTransform.localScale = Vector3.zero;
    }

    // 전달받은 UpgradeData를 기반으로 UI 구성을 초기화하는 메서드
    private void InitUI(UpgradeData data)
    {
        upgradeData = data;
        SetUpgradeCardUI();
    }

    // 업그레이드 카드의 UI 요소를 설정
    private void SetUpgradeCardUI()
    {
        if (upgradeData == null) return;
        titleText.text = upgradeData.UpgradeName;
        iconImage.sprite = upgradeData.Icon;
        descriptionText.text = upgradeData.Description;
    }

    public Tween CardOpen()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(canvasGroup.DOFade(1f, 0.25f));
        sequence.Join(rectTransform.DOAnchorPos(originalPosition, 0.35f).SetEase(Ease.OutCubic));
        sequence.Join(rectTransform.DORotateQuaternion(originalRotation, 0.35f).SetEase(Ease.OutCubic));
        sequence.Join(rectTransform.DOScale(originalScale, 0.35f).SetEase(Ease.OutBack));
        return sequence;
    }

    public void SelectCard()
    {
        if (!isClickable) return;
        OnCardSelected?.Invoke(upgradeData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClickable) return;

        // 현재 진행중인 애니메이션이 있다면 중지
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClickable) return;
        // 현재 진행중인 애니메이션이 있다면 중지
        transform.DOKill();
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack);
    }
}
