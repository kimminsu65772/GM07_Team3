using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStats
{
    //private readonly Dictionary<StatType, float> stats = new();

    private readonly Dictionary<StatType, float> baseStats = new();
    private readonly Dictionary<StatType, float> itemStats = new();
    private readonly Dictionary<StatType, float> totalStats = new();

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
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            if (statType == StatType.None)
            {
                continue;
            }

            float defaultValue = GetDefaultValue(statType);

            baseStats[statType] = defaultValue;
            itemStats[statType] = 0f;
            totalStats[statType] = defaultValue;
        }
        foreach (StatEntry statEntry in playerStatData.BaseStats)
        {
            StatType statType = statEntry.StatType;

            if (!IsValidStatType(statType))
            {
                Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
                continue;
            }
            baseStats[statType] = ClampStat(statType, statEntry.Value);
        }
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            if (statType == StatType.None)
            {
                continue;
            }

            RecalculateTotalStat(statType);
        }
    }

    public float GetBaseStat(StatType statType)
    {
        if (!IsValidStatType(statType))
        {
            Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
            return 0f;
        }
        return baseStats[statType];
    }

    public float GetItemStat(StatType statType)
    {
        if (!IsValidStatType(statType))
        {
            Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
            return 0f;
        }
        return itemStats[statType];
    }
    public float GetTotalStat(StatType statType)
    {
        if (!IsValidStatType(statType))
        {
            Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
            return 0f;
        }
        return totalStats[statType];
    }
    //기본 스탯 증가 메서드 ex-레벨업? 혹시 몰라서만듦
    public void AddBaseStat(StatType statType, float amount)
    {
        if (!IsValidStatType(statType))
        {
            Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
            return;
        }

        float newBaseValue = baseStats[statType] + amount;

        baseStats[statType] = ClampStat(statType, newBaseValue);

        RecalculateTotalStat(statType);
    }
    public void AddItemStat(StatType statType, float amount)
    {
        if (!IsValidStatType(statType))
        {
            Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
            return;
        }

        itemStats[statType] += amount;

        RecalculateTotalStat(statType);
    }
    //아이템 스탯 초기화용 어찌 쓰일지 모름
    public void ClearItemStats(StatType statType)
    {
        if (!IsValidStatType(statType))
        {
            Debug.LogWarning($"{statType}은 유효한 스탯이 아닙니다.");
            return;
        }
        itemStats[statType] = 0f;

        RecalculateTotalStat(statType);
    }
    //모든 아이템 스탯 초기화용
    public void ClearAllAdditionalStats()
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            if (statType == StatType.None)
            {
                continue;
            }
            itemStats[statType] = 0f;

            RecalculateTotalStat(statType);
        }
    }
    private void RecalculateTotalStat(StatType statType)
    {
        float calculatedValue = baseStats[statType] + itemStats[statType];

        totalStats[statType] = ClampStat(statType, calculatedValue);
    }

    private static bool IsValidStatType(StatType statType)
    {
        return statType > StatType.None && Enum.IsDefined(typeof(StatType), statType);
    }
    private static float GetDefaultValue(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHp:
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
            case StatType.MaxHp:
                return Mathf.Max(1f, value);

            case StatType.Defense:
                return Mathf.Max(0f, value);

            case StatType.Damage:
                return Mathf.Max(0f, value);

            case StatType.AttackSpeed:
                return Mathf.Max(0.1f, value);

            case StatType.Critical:
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