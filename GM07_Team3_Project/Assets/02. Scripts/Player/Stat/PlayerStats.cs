using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStats
{
    //private readonly Dictionary<StatType, float> stats = new();

    private readonly Dictionary<StateType, float> baseStats = new();
    private readonly Dictionary<StateType, float> itemStats = new();
    private readonly Dictionary<StateType, float> totalStats = new();

    //public event Action<StatType, float, float> OnStatChanged;

    public PlayerStats(PlayerStatSO playerStatData)
    {
        if (playerStatData == null)
        {
            throw new ArgumentNullException(nameof(playerStatData));
        }
        InitializeStats(playerStatData);
    }

    private void InitializeStats(PlayerStatSO playerStatData)
    {
        foreach (StateType stateType in Enum.GetValues(typeof(StateType)))
        {
            if (stateType == StateType.None)
            {
                continue;
            }

            float defaultValue = GetDefaultValue(stateType);

            baseStats[stateType] = defaultValue;
            itemStats[stateType] = 0f;
            totalStats[stateType] = defaultValue;
        }
        foreach (StatEntry statEntry in playerStatData.BaseStats)
        {
            StateType stateType = statEntry.StateType;

            if (!IsValidStatType(stateType))
            {
                Debug.LogWarning($"{stateType}은 유효한 스탯이 아닙니다.");
                continue;
            }
            baseStats[stateType] = ClampStat(stateType, statEntry.Value);
        }
        foreach (StateType stateType in Enum.GetValues(typeof(StateType)))
        {
            if (stateType == StateType.None)
            {
                continue;
            }

            RecalculateTotalStat(stateType);
        }
    }

    public float GetBaseStat(StateType stateType)
    {
        if (!IsValidStatType(stateType))
        {
            Debug.LogWarning($"{stateType}은 유효한 스탯이 아닙니다.");
            return 0f;
        }
        return baseStats[stateType];
    }

    public float GetItemStat(StateType stateType)
    {
        if (!IsValidStatType(stateType))
        {
            Debug.LogWarning($"{stateType}은 유효한 스탯이 아닙니다.");
            return 0f;
        }
        return itemStats[stateType];
    }
    public float GetTotalStat(StateType stateType)
    {
        if (!IsValidStatType(stateType))
        {
            Debug.LogWarning($"{stateType}은 유효한 스탯이 아닙니다.");
            return 0f;
        }
        return totalStats[stateType];
    }
    //기본 스탯 증가 메서드 ex-레벨업? 혹시 몰라서만듦
    public void AddBaseStat(StateType stateType, float amount)
    {
        if (!IsValidStatType(stateType))
        {
            Debug.LogWarning($"{stateType}은 유효한 스탯이 아닙니다.");
            return;
        }

        float newBaseValue = baseStats[stateType] + amount;

        baseStats[stateType] = ClampStat(stateType, newBaseValue);

        RecalculateTotalStat(stateType);
    }
    public void AddItemStat(StateType stateType, float amount)
    {
        if (!IsValidStatType(stateType))
        {
            Debug.LogWarning($"{stateType}은 유효한 스탯이 아닙니다.");
            return;
        }

        itemStats[stateType] += amount;

        RecalculateTotalStat(stateType);
    }
    //아이템 스탯 초기화용 어찌 쓰일지 모름
    public void ClearItemStats(StateType stateType)
    {
        if (!IsValidStatType(stateType))
        {
            Debug.LogWarning($"{stateType}은 유효한 스탯이 아닙니다.");
            return;
        }
        itemStats[stateType] = 0f;

        RecalculateTotalStat(stateType);
    }
    //모든 아이템 스탯 초기화용
    public void ClearAllAdditionalStats()
    {
        foreach (StateType stateType in Enum.GetValues(typeof(StateType)))
        {
            if (stateType == StateType.None)
            {
                continue;
            }
            itemStats[stateType] = 0f;

            RecalculateTotalStat(stateType);
        }
    }
    private void RecalculateTotalStat(StateType stateType)
    {
        float calculatedValue = baseStats[stateType] + itemStats[stateType];

        totalStats[stateType] = ClampStat(stateType, calculatedValue);
    }

    private static bool IsValidStatType(StateType stateType)
    {
        return stateType > StateType.None && Enum.IsDefined(typeof(StateType), stateType);
    }
    private static float GetDefaultValue(StateType stateType)
    {
        switch (stateType)
        {
            case StateType.MaxHp:
                return 1f;

            case StateType.AttackSpeed:
                return 1f;

            default:
                return 0f;
        }
    }

    private static float ClampStat(StateType stateType, float value)
    {
        switch (stateType)
        {
            case StateType.MaxHp:
                return Mathf.Max(1f, value);

            case StateType.Defense:
                return Mathf.Max(0f, value);

            case StateType.Damage:
                return Mathf.Max(0f, value);

            case StateType.AttackSpeed:
                return Mathf.Max(0.1f, value);

            case StateType.Critical:
                return Mathf.Max(0f, value);

            default:
                return value;
        }
    }

    //private static float ClampBaseStat(StatType statType, float value)
    //{
    //    switch (statType)
    //    {
    //        case StatType.MaxHealth:
    //            return Mathf.Max(1f, value);

    //        case StatType.Defense:
    //            return Mathf.Max(0f, value);

    //        case StatType.AttackPower:
    //            return Mathf.Max(0f, value);

    //        case StatType.AttackSpeed:
    //            return Mathf.Max(0.1f, value);

    //        case StatType.CriticalChance:
    //            return Mathf.Max(0f, value);

    //        default:
    //            return value;
    //    }
    //}
    //private static float ClampTotalStat(StatType statType, float value)
    //{
    //    switch (statType)
    //    {
    //        case StatType.MaxHealth:
    //            return Mathf.Max(1f, value);

    //        case StatType.Defense:
    //            return Mathf.Max(0f, value);

    //        case StatType.AttackPower:
    //            return Mathf.Max(0f, value);

    //        case StatType.AttackSpeed:
    //            return Mathf.Max(0.1f, value);

    //        case StatType.CriticalChance:
    //            return Mathf.Max(0f, value);

    //        default:
    //            return value;
    //    }
    //}

    /*
    public float GetStat(StatType statType)
    {
        if (!IsValidStatType(statType))
        {
            Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
            return 0f;
        }
        //return stats[statType];
        return GetTotalStat(statType);
    }

    public void AddStat(StatType statType, float amount)
    {
        if (!IsValidStatType(statType))
        {
            Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
            return;
        }
        float previousValue = stats[statType];

        float newValue = previousValue + amount;

        newValue = ClampStat(statType, newValue);

        if (Mathf.Approximately(previousValue, newValue)) // 비교 생각필요
        {
            return;
        }
        stats[statType] = newValue;

        OnStatChanged?.Invoke(statType, previousValue, newValue);
    }

    private static bool IsValidStatType(StatType statType)
    {
        return statType > StatType.None && statType < StatType.Length;
    }

    private static float GetDefaultValue(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                return 1f;

            case StatType.AttackSpeed:
                return 1f;

            default:
                return 0f;
        }
    }

    private static float ClampStat(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                return Mathf.Max(1f, value);

            case StatType.Defense:
                return Mathf.Max(0f, value);

            case StatType.AttackPower:
                return Mathf.Max(0f, value);

            case StatType.AttackSpeed:
                return Mathf.Max(0.1f, value);

            case StatType.CriticalChance:
                return Mathf.Max(0f, value);

            default:
                return value;
        }
    }*/
}