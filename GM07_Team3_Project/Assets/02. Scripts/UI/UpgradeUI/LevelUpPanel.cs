using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LevelUpPanel : MonoBehaviour
{
    [Header("제어할 업그레이드 패널 설정")]
    [SerializeField] private UpgradePanel upgradePanel;

    private RectTransform rectTransform;
    private CanvasGroup levelUpCanvasGroup;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        levelUpCanvasGroup = GetComponent<CanvasGroup>();
        originalScale = rectTransform.localScale;
    }

    public void OpenLevelUpPanel(List<UpgradeOption> upgradeCards)
    {
        Initialize();

        levelUpCanvasGroup.alpha = 1f;
        levelUpCanvasGroup.interactable = true;
        levelUpCanvasGroup.blocksRaycasts = true;
        
        rectTransform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);

        // 업그레이드 카드 그룹 등장 애니메이션 수행
        upgradePanel.OpenUpgradePanel(upgradeCards);
    }

    public void CloseLevelUpPanel()
    {
        rectTransform.DOKill();
        rectTransform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            Initialize();
            gameObject.SetActive(false);
        });
    }

    // 레벨업 패널을 열고 닫을 때 패널을 기본 상태로 초기화하는 메서드
    private void Initialize()
    {
        rectTransform.DOKill();
        rectTransform.localScale = Vector3.zero;
        levelUpCanvasGroup.alpha = 0f;
        levelUpCanvasGroup.interactable = false;
        levelUpCanvasGroup.blocksRaycasts = false;
    }
}
