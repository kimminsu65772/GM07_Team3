using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UpgradeCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UpgradeOption UpgradeData;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image[] cardFrameImages;

    [Header("Rarity Frame Colors")]
    [SerializeField] private Color commonFrameColor = new Color(0.18f, 0.58f, 1f);
    [SerializeField] private Color uncommonFrameColor = new Color(0.25f, 0.95f, 0.55f);
    [SerializeField] private Color rareFrameColor = new Color(1f, 0.75f, 0.18f);

    // 버튼에 마우스가 올라갔을 때의 스케일을 정의하는 변수
    [Header("Animation Scale Setting")]
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float pressScale = 0.9f;

    [Header("Start YOffset Seting")]
    [SerializeField] private float startYOffset = -80f;

    [Header("Start Y Rotation Setting")]
    [SerializeField] private float startYRotation = 180f;

    public Action<UpgradeOption> OnCardSelected;

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
    }

    private void OnDisable()
    {
        Initialize();
        isClickable = false;
        UpgradeData = null;
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

    // 업그레이드 데이터를 설정하는 메서드
    public void SetUpgradeData(UpgradeOption data)
    {
        UpgradeData = data;
    }

    // 업그레이드 카드의 UI 요소를 설정
    private void SetUpgradeCardUI()
    {
        if (UpgradeData == null) return;
        titleText.text = UpgradeData.Data.UpgradeName;
        iconImage.sprite = UpgradeData.Data.Icon;
        descriptionText.text = UpgradeData.Data.Description + $" {UpgradeData.Value:F1}";
        SetCardFrameColor(UpgradeData.Rarity);
    }

    private void SetCardFrameColor(UpgradeRarity rarity)
    {
        if (cardFrameImages == null) return;

        Color frameColor = GetFrameColor(rarity);

        for (int i = 0; i < cardFrameImages.Length; i++)
        {
            if (cardFrameImages[i] == null) continue;

            cardFrameImages[i].color = frameColor;
        }
    }

    private Color GetFrameColor(UpgradeRarity rarity)
    {
        switch (rarity)
        {
            case UpgradeRarity.Uncommon:
                return uncommonFrameColor;
            case UpgradeRarity.Rare:
                return rareFrameColor;
            default:
                return commonFrameColor;
        }
    }

    public Tween CardOpen()
    {
        SetUpgradeCardUI();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        rectTransform.DOKill();
        transform.DOKill();
        Sequence sequence = DOTween.Sequence();
        sequence.Join(canvasGroup.DOFade(1f, 0.25f));
        sequence.Join(rectTransform.DOAnchorPos(originalPosition, 0.35f).SetEase(Ease.OutCubic));
        sequence.Join(rectTransform.DORotateQuaternion(originalRotation, 0.35f).SetEase(Ease.OutCubic));
        sequence.Join(rectTransform.DOScale(originalScale, 0.35f).SetEase(Ease.OutBack));
        sequence.SetUpdate(true);

        return sequence;
    }

    public void SelectCard()
    {
        if (!isClickable) return;
        OnCardSelected?.Invoke(UpgradeData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClickable) return;

        // 현재 진행중인 애니메이션이 있다면 중지
        transform.DOKill();
        rectTransform.DOKill();
        transform.DOScale(originalScale * hoverScale, 0.2f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClickable) return;
        // 현재 진행중인 애니메이션이 있다면 중지
        transform.DOKill();
        rectTransform.DOKill();
        transform.DOScale(originalScale, 0.2f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
}
