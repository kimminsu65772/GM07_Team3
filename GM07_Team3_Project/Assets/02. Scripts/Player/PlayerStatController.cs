using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStatController : MonoBehaviour
{
    [Header("플레이어 기본 스탯 데이터")]
    [SerializeField] private PlayerStatSO playerStatData;
    [SerializeField] private PlayerLevelSO playerLevelData;

    [Header("런타임 스탯 확인용")]
    [SerializeField] private List<RuntimeStatEntry> runtimeStats = new();
    [SerializeField] private int runtimeLevel;
    [SerializeField] private int runtimeExperience;

    private PlayerStats playerStats;
    private PlayerLevel playerLevel;

    //public event Action<StatType, float, float> OnStatChanged;

    private void Awake()
    {
        if (playerStatData == null)
        {
            Debug.LogError($"{name}: PlayerStatSO가 등록되지 않았습니다.", this);
            enabled = false;
            return;
        }
        if (playerLevelData == null)
        {
            Debug.LogError($"{name}: playerLevelData가 등록되지 않았습니다.", this);
            enabled = false;
            return;
        }

        playerStats = new PlayerStats(playerStatData);
        playerLevel = new PlayerLevel(playerLevelData);

        RuntimeStat();
        UpdateRuntimeLevel();
    }

    private void OnEnable()
    {
        //UpgradeEventManager.OnUpgradeSelected += HandleUpgradeSelected;
    }

    private void OnDisable()
    {
        //UpgradeEventManager.OnUpgradeSelected -= HandleUpgradeSelected;
    }

    public float GetStat(StatType statType)
    {
        if (!CheckPlayerStats())
        {
            Debug.LogError($"{name}: PlayerStats가 초기화되지 않았습니다.", this);
            return 0f;
        }
        return playerStats.GetTotalStat(statType);
    }

    //private void HandleUpgradeSelected(UpgradeData upgradeData)
    //{
    //    if (upgradeData == null)
    //    {
    //        Debug.LogWarning($"{name}: 선택된 UpgradeData가 없습니다.", this);

    //        return;
    //    }

    //    if (upgradeData.StatType == StatType.None)
    //    {
    //        return;
    //    }
    //    playerStats.AddItemStat(upgradeData.StatType, upgradeData.Value);

    //    UpdateRuntimeStat(upgradeData.StatType);
    //}

    private void HandleExperience(int amount)
    {
        int previousLevel = playerLevel.CurrentLevel;

        int levelUpCount = playerLevel.AddExperience(amount);

        UpdateRuntimeLevel();

        //for (int i = 0; i < levelUpCount; i++)
        //{
        //    int newLevel = previousLevel + i + 1;

        //    HandleLevelUp(newLevel);
        //}
    }



    private void RuntimeStat()
    {
        runtimeStats.Clear();

        for (int i = 0; i < (int)StatType.Length; i++)
        {
            StatType statType = (StatType)i;

            RuntimeStatEntry runtimeStat =
                new RuntimeStatEntry(
                    statType,
                    playerStats.GetBaseStat(statType),
                    playerStats.GetItemStat(statType),
                    playerStats.GetTotalStat(statType));

            runtimeStats.Add(runtimeStat);
        }
    }

    private void UpdateRuntimeStat(
    StatType statType)
    {
        foreach (RuntimeStatEntry runtimeStat
                 in runtimeStats)
        {
            if (runtimeStat.StatType != statType)
            {
                continue;
            }

            runtimeStat.SetValues(
                playerStats.GetBaseStat(statType),
                playerStats.GetItemStat(statType),
                playerStats.GetTotalStat(statType));

            return;
        }
    }

    private bool CheckPlayerStats()
    {
        if (playerStats != null)
        {
            return true;
        }
        Debug.LogError($"{name}: PlayerStats가 초기화되지 않았습니다.", this);

        return false;
    }

    private void UpdateRuntimeLevel()
    {
        runtimeLevel = playerLevel.CurrentLevel;

        runtimeExperience = playerLevel.CurrentExperience;
    }
}


[Serializable]
public sealed class RuntimeStatEntry
{
    [SerializeField]
    private StatType statType;

    [SerializeField]
    private float baseValue;

    [SerializeField]
    private float itemValue;

    [SerializeField]
    private float totalValue;

    public StatType StatType => statType;

    public float BaseValue => baseValue;

    public float ItemValue => itemValue;

    public float TotalValue => totalValue;

    public RuntimeStatEntry(StatType statType, float baseValue, float itemValue, float totalValue)
    {
        this.statType = statType;
        this.baseValue = baseValue;
        this.itemValue = itemValue;
        this.totalValue = totalValue;
    }

    public void SetValues(float newBaseValue, float newAdditionalValue, float newTotalValue)
    {
        baseValue = newBaseValue;
        itemValue = newAdditionalValue;
        totalValue = newTotalValue;
    }
}


/*

public float GetBaseStat(StatType statType)
    {
        if (!CheckPlayerStats())
        {
            return 0f;
        }
        return playerStats.GetBaseStat(statType);
    }
    public float GetItemStat(StatType statType)
    {
        if (!CheckPlayerStats())
        {
            return 0f;
        }
        return playerStats.GetItemStat(statType);
    }
    public float GetTotalStat(StatType statType)
    {
        if (!CheckPlayerStats())
        {
            return 0f;
        }
        return playerStats.GetTotalStat(statType);
    }
    public void AddBaseStat(StatType statType, float amount)
    {
        if (!CheckPlayerStats())
        {
            return;
        }
        float previousTotalValue = playerStats.GetTotalStat(statType);

        playerStats.AddBaseStat(statType, amount);

        float currentTotalValue = playerStats.GetTotalStat(statType);

        NotifyStatChanged(statType, previousTotalValue, currentTotalValue);
    }

    public void AddItemStat(StatType statType, float amount)
    {
        if (!CheckPlayerStats())
        {
            return;
        }
        float previousTotalValue = playerStats.GetTotalStat(statType);

        playerStats.AddItemStat(statType, amount);

        float currentTotalValue = playerStats.GetTotalStat(statType);

        NotifyStatChanged(statType, previousTotalValue, currentTotalValue);
    }




    private void NotifyStatChanged(StatType statType, float previousValue, float currentValue)
    {
        if (Mathf.Approximately(previousValue, currentValue))
        {
            return;
        }
        UpdateRuntimeStat(statType);

        OnStatChanged?.Invoke(statType, previousValue, currentValue);
    }
 */