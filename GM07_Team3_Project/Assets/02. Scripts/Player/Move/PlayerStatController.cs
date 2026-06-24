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

    public float GetStat(StateType stateType)
    {
        if (!CheckPlayerStats())
        {
            Debug.LogError($"{name}: PlayerStats가 초기화되지 않았습니다.", this);
            return 0f;
        }
        return playerStats.GetTotalStat(stateType);
    }

    //private void HandleUpgradeSelected(UpgradeOption upgradeOption)
    //{
    //    if (upgradeOption == null)
    //    {
    //        Debug.LogWarning($"{name}: 선택된 UpgradeOption이 없습니다.", this);

    //        return;
    //    }

    //    if (upgradeOption.Data == null)
    //    {
    //        Debug.LogWarning($"{name}: UpgradeOption에 UpgradeData가 없습니다.", this);

    //        return;
    //    }

    //    StateType stateType =  upgradeOption.Data.StateType;

    //    if (stateType == StateType.None)
    //    {
    //        return;
    //    }

    //    AddItemStat(stateType, upgradeOption.Value);
    //}
    //public void AddItemStat(StateType stateType, float amount)
    //{
    //    if (!CheckPlayerStats())
    //    {
    //        return;
    //    }
    //    float stateValue = playerStats.GetTotalStat(stateType);

    //    playerStats.AddItemStat(stateType, amount);

    //    float totalValue = playerStats.GetTotalStat(stateType);

    //    StatChanged(stateType, stateValue, totalValue);
    //}

    //private void StatChanged(StateType stateType, float stateValue, float totalValue)
    //{
    //    if (Mathf.Approximately(stateValue, totalValue))
    //    {
    //        return;
    //    }
    //    UpdateRuntimeStat(stateType);

    //    OnStatChanged?.Invoke(stateType, stateValue, totalValue);
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

        foreach (StateType stateType in Enum.GetValues(typeof(StateType)))
        {
            if (stateType == StateType.None)
            {
                continue;
            }
            RuntimeStatEntry runtimeStat = new RuntimeStatEntry(
                stateType,
                playerStats.GetBaseStat(stateType),
                playerStats.GetItemStat(stateType),
                playerStats.GetTotalStat(stateType));

            runtimeStats.Add(runtimeStat);
        }
    }

    private void UpdateRuntimeStat(StateType stateType)
    {
        foreach (RuntimeStatEntry runtimeStat in runtimeStats)
        {
            if (runtimeStat.StatType != stateType)
            {
                continue;
            }

            runtimeStat.SetValues(
                playerStats.GetBaseStat(stateType),
                playerStats.GetItemStat(stateType),
                playerStats.GetTotalStat(stateType));

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
    private StateType stateType;

    [SerializeField]
    private float baseValue;

    [SerializeField]
    private float itemValue;

    [SerializeField]
    private float totalValue;

    public StateType StatType => stateType;

    public float BaseValue => baseValue;

    public float ItemValue => itemValue;

    public float TotalValue => totalValue;

    public RuntimeStatEntry(StateType stateType, float baseValue, float itemValue, float totalValue)
    {
        this.stateType = stateType;
        this.baseValue = baseValue;
        this.itemValue = itemValue;
        this.totalValue = totalValue;
    }

    public void SetValues(float newBaseValue, float newAdditemValue, float newTotalValue)
    {
        baseValue = newBaseValue;
        itemValue = newAdditemValue;
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







 */