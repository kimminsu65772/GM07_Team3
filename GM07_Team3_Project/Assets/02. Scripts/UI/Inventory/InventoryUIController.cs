using TMPro;
using UnityEngine;
using DG.Tweening;

public sealed class InventoryUIController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float slideDuration = 0.15f;
    [SerializeField] private float slideDistance = 750f;

    [Header("Panel")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Player References")]
    [SerializeField] private PlayerStatController playerStatController;
    [SerializeField] private ItemStatManager itemStatManager;

    [Header("Inventory Slots")]
    [SerializeField] private InventoryIconSlot[] itemSlots;
    [SerializeField] private InventoryIconSlot[] statSlots;

    [Header("Item Stat Texts")]
    [SerializeField] private TextMeshProUGUI damageBonusText;
    [SerializeField] private TextMeshProUGUI attackSpeedBonusText;
    [SerializeField] private TextMeshProUGUI criticalChanceBonusText;
    [SerializeField] private TextMeshProUGUI damagePercentBonusText;

    [Header("Player Stat Texts")]
    [SerializeField] private TextMeshProUGUI maxHpText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;

    private RectTransform rectTransform;
    private bool isOpen;
    private float openedXPosition;
    private float closedXPosition;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        rectTransform = canvasGroup.GetComponent<RectTransform>();
        openedXPosition = rectTransform.anchoredPosition.x;
        closedXPosition = openedXPosition - slideDistance;

        rectTransform.anchoredPosition = new Vector2(closedXPosition, rectTransform.anchoredPosition.y);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false);

        ClearAllSlots();
    }

    public void ToggleInventoryPanel()
    {
        if (isOpen)
        {
            CloseInventoryPanel();
            return;
        }

        OpenInventoryPanel();
    }

    public void OpenInventoryPanel()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        RefreshInventory();
        Open();
    }

    public void CloseInventoryPanel()
    {
        if (!isOpen)
        {
            return;
        }

        isOpen = false;
        Close();
    }

    public void RefreshInventory()
    {
        ClearAllSlots();
        RefreshIconSlots();
        RefreshTotalStats();
    }

    private void Open()
    {
        rectTransform.DOKill();

        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        rectTransform.anchoredPosition = new Vector2(closedXPosition, rectTransform.anchoredPosition.y);
        rectTransform
            .DOAnchorPosX(openedXPosition, slideDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
    }

    private void Close()
    {
        rectTransform.DOKill();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        rectTransform
            .DOAnchorPosX(closedXPosition, slideDuration)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                canvasGroup.alpha = 0f;
                canvasGroup.gameObject.SetActive(false);
            });
    }

    private void RefreshIconSlots()
    {
        if (playerStatController == null)
        {
            Debug.LogWarning($"{name}: PlayerStatController is not assigned.", this);
            return;
        }

        int itemSlotIndex = 0;
        int statSlotIndex = 0;

        foreach (var item in playerStatController.ItemList)
        {
            UpgradeData upgradeData = item.Key;
            int count = item.Value;

            if (upgradeData == null)
            {
                continue;
            }

            switch (upgradeData.UpgradeType)
            {
                case UpgradeType.Weapon:
                    TrySetSlot(itemSlots, ref itemSlotIndex, upgradeData, count);
                    break;

                case UpgradeType.Stat:
                    TrySetSlot(statSlots, ref statSlotIndex, upgradeData, count);
                    break;
            }
        }
    }

    private static void TrySetSlot(InventoryIconSlot[] slots, ref int slotIndex, UpgradeData upgradeData, int count)
    {
        if (slots == null || slotIndex >= slots.Length)
        {
            return;
        }

        InventoryIconSlot slot = slots[slotIndex];
        slotIndex++;

        if (slot == null)
        {
            return;
        }

        slot.SetSlot(upgradeData, count);
    }

    private void RefreshTotalStats()
    {
        RefreshItemStats();
        RefreshPlayerStats();
    }

    private void RefreshItemStats()
    {
        if (itemStatManager == null)
        {
            Debug.LogWarning($"{name}: ItemStatManager is not assigned.", this);
            return;
        }

        SetText(damageBonusText, FormatSignedNumber(itemStatManager.DamageBonus));
        SetText(attackSpeedBonusText, FormatPercent01(itemStatManager.AttackSpeedBonus));
        SetText(criticalChanceBonusText, FormatPercent100(itemStatManager.CriticalChanceBonus));
        SetText(damagePercentBonusText, FormatPercent01(itemStatManager.DamagePercentBonus));
    }

    private void RefreshPlayerStats()
    {
        if (playerStatController == null)
        {
            return;
        }

        SetText(maxHpText, FormatNumber(playerStatController.GetStat(StatType.MaxHp)));
        SetText(defenseText, FormatNumber(playerStatController.GetStat(StatType.Defense)));
        SetText(moveSpeedText, FormatNumber(playerStatController.GetStat(StatType.MoveSpeed)));
    }

    private void ClearAllSlots()
    {
        ClearSlots(itemSlots);
        ClearSlots(statSlots);
    }

    private static void ClearSlots(InventoryIconSlot[] slots)
    {
        if (slots == null)
        {
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            slots[i].ClearSlot();
        }
    }

    private static void SetText(TextMeshProUGUI text, string value)
    {
        if (text == null)
        {
            return;
        }

        text.text = value;
    }

    private static string FormatNumber(float value)
    {
        return value.ToString("0.#");
    }

    private static string FormatSignedNumber(float value)
    {
        return value >= 0f ? $"+{value:0.#}" : value.ToString("0.#");
    }

    private static string FormatPercent01(float value)
    {
        float percentValue = value * 100f;
        return percentValue >= 0f ? $"+{percentValue:0.#}%" : $"{percentValue:0.#}%";
    }

    private static string FormatPercent100(float value)
    {
        return value >= 0f ? $"+{value:0.#}%" : $"{value:0.#}%";
    }

    private void OnDestroy()
    {
        if (rectTransform != null)
        {
            rectTransform.DOKill();
        }
    }
}
