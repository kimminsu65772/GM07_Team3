using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    [Header("Player HUD 세팅")]
    [SerializeField] private ExpBar expBar;
    [SerializeField] private HPBar hpBar;
    [SerializeField] private InventoryIconSlot[] itemSlots;

    [SerializeField] PlayerStatController playerStatController;

    private int currentExp = 0;
    private int requiredExp;
    private int currentLevel = 1;

    private float maxHp;
    private float currentHp;

    private void Start()
    {
        maxHp = playerStatController.GetStat(StatType.MaxHp);
        currentHp = maxHp;
        HandleHpBar(currentHp, maxHp);
        requiredExp = playerStatController.GetRequiredExperience();
        HandleExpBar(currentExp, requiredExp);
        HandleLevelText(currentLevel);


        playerStatController.OnExperienceChanged += HandleExpBar;
        playerStatController.OnHealthChanged += HandleHpBar;
        playerStatController.OnLevelChanged += HandleLevelChanged;
        playerStatController.OnItemListChanged += RefreshItemSlots;

        ClearSlots();
    }

    private void OnDisable()
    {
        playerStatController.OnExperienceChanged -= HandleExpBar;
        playerStatController.OnHealthChanged -= HandleHpBar;
        playerStatController.OnLevelChanged -= HandleLevelChanged;
        playerStatController.OnItemListChanged -= RefreshItemSlots;
    }

    private void HandleExpBar(int currentExp, int requiredExp)
    {
        expBar.SetExpBar(currentExp, requiredExp);
    }

    private void HandleLevelText(int currentLevel)
    {
        expBar.ChangeLevelText(currentLevel);
    }

    private void HandleLevelChanged(int currentLevel)
    {
        expBar.ChangeLevelText(currentLevel);
        expBar.PlayLevelUpEffect();
    }

    private void HandleHpBar(float currentHp, float MaxHp)
    {
        hpBar.SetHPBar(currentHp, MaxHp);
    }

    private void RefreshItemSlots(IReadOnlyDictionary<UpgradeData, int> itemList)
    {
        ClearSlots();

        int slotIndex = 0;

        foreach (var item in playerStatController.ItemList)
        {
            if (slotIndex >= itemSlots.Length)
            {
                break;
            }

            UpgradeData data = item.Key;

            if (data.UpgradeType != UpgradeType.Weapon)
            {
                continue;
            }
            int count = item.Value;
            itemSlots[slotIndex].SetSlot(data, count);
            slotIndex++;
        }
    }

    private void ClearSlots()
    {
        foreach (var slot in itemSlots)
        {
            if (slot == null)
            {
                continue;
            }
            slot.ClearSlot();
        }
    }
}
