using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    // UI 매니저로부터 전달받은 랜덤 업그레이드 카드 리스트
    [SerializeField] private List<UpgradeData> currentCardList;

    // 업그레이드 카드를 배열로 저장
    [SerializeField] private UpgradeCard[] upgradeCardSlots;

    [SerializeField] private UpgradeUIController upgradeUIController;

    private CanvasGroup canvasGroup;
    private UpgradeData selectedCard;

    

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        InitializePanel();
    }

    private void OnDisable()
    {
        InitializePanel();
    }

    public void OpenUpgradePanel(List<UpgradeData> upgradeCards)
    {

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        SetUpgradeCards(upgradeCards);
        Sequence sequence = DOTween.Sequence();
        // 업그레이드 카드 등장 애니메이션을 순차적으로 실행
        for (int i = 0; i < upgradeCardSlots.Length; i++)
        {
            int index = i;
            // 업그레이드 카드에 있는 등장 애니메이션을 실행하는 메서드를 Append로 시퀀스에 추가
            sequence.AppendCallback(() =>
            {
                upgradeCardSlots[index].CardOpen();
            });
            sequence.AppendInterval(0.2f);
        }
    }

    private void InitializePanel()
    {
        selectedCard = null;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;   
        canvasGroup.blocksRaycasts = false;

        for (int i = 0; i < upgradeCardSlots.Length; i++)
        {
            upgradeCardSlots[i].OnCardSelected -= HandleSelectedCard;
        }
    }

    private void SetUpgradeCards(List<UpgradeData> cards)
    {
        currentCardList = cards;

        // 업그레이드 카드 슬롯과 업그레이드 카드 리스트의 길이가 일치하지 않으면 에러 메시지를 출력하고 함수를 종료
        if (!(upgradeCardSlots.Length == currentCardList.Count))
        {
            Debug.LogError("업그레이드 카드 슬롯과 업그레이드 카드 리스트의 길이가 일치하지 않습니다.");
            return;
        }

        for (int i = 0; i < upgradeCardSlots.Length; i++)
        {
            if (i < currentCardList.Count)
            {
                upgradeCardSlots[i].gameObject.SetActive(true);
                upgradeCardSlots[i].SetUpgradeData(currentCardList[i]);
                upgradeCardSlots[i].OnCardSelected += HandleSelectedCard;
            }
        }
    }

    private void HandleSelectedCard(UpgradeData card)
    {
        selectedCard = card;
        upgradeUIController.UpgradeCardSelected(selectedCard);
    }
}
