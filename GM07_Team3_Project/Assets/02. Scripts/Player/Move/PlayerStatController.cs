using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStatController : MonoBehaviour, IDamageable
{
    [Header("플레이어 기본 스탯 데이터")]
    [SerializeField] private PlayerStatSO playerStatData;
    [SerializeField] private PlayerLevelSO playerLevelData;

    //[Header("시작 무기 설정")]
    //[SerializeField] private WeaponBase weaponBase;
    //[SerializeField] private UpgradeData startWeapon;

    [Header("런타임 스탯 확인용")]
    [SerializeField] private List<RuntimeStatEntry> runtimeStats = new();
    [SerializeField] private int runtimeLevel;
    [SerializeField] private int runtimeExperience;
    [SerializeField] private float currentHealth;

    

    public float CurrentHealth => currentHealth;
    public float MaxHealth => GetStat(StatType.MaxHp);
    public bool IsDead { get; private set; }

    private PlayerStats playerStats;
    private PlayerLevel playerLevel;

    public event Action<StatType, float, float> OnStateChanged;
    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int, int> OnExperienceChanged;
    public event Action OnDied;


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
        currentHealth = MaxHealth;

        //UpgradeOption option = new UpgradeOption(startWeapon, startWeapon.Value);
        //weaponBase.Init(option, transform);

        RuntimeStat();
        UpdateRuntimeLevel();

        
    }

    private void OnEnable()
    {
        UpgradeEventManager.Instance.OnUpgradeSelected += HandleUpgradeSelected;
    }

    private void OnDisable()
    {
        UpgradeEventManager.Instance.OnUpgradeSelected -= HandleUpgradeSelected;
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

    private void HandleUpgradeSelected(UpgradeOption upgradeOption)
    {
        if (upgradeOption == null)
        {
            Debug.LogWarning($"{name}: 선택된 UpgradeOption이 없습니다.", this);

            return;
        }

        if (upgradeOption.Data == null)
        {
            Debug.LogWarning($"{name}: UpgradeOption에 UpgradeData가 없습니다.", this);

            return;
        }

        Debug.Log($"{name}: UpgradeOption 선택됨 - {upgradeOption.Data.name}, Value: {upgradeOption.Value}", this);

        // weaponBase.Init(upgradeOption, transform);

        StatType statType = upgradeOption.Data.StatType;

        if (statType == StatType.None)
        {
            Debug.LogWarning($"{name}: UpgradeOption의 StatType이 None입니다. 스탯 변경이 적용되지 않습니다.", this);
            return;
        }

        AddItemStat(statType, upgradeOption.Value);
    }
    public void AddItemStat(StatType statType, float amount)
    {
        if (!CheckPlayerStats())
        {
            return;
        }

        Debug.Log($"{name}: AddItemStat 호출 - StatType: {statType}, Amount: {amount}", this);

        float previousValue = playerStats.GetTotalStat(statType);

        playerStats.AddItemStat(statType, amount);

        float currentValue = playerStats.GetTotalStat(statType);

        StatChanged(statType, previousValue, currentValue);
    }

    private void StatChanged(StatType StatType, float previousValue, float currentValue)
    {
        Debug.Log($"{name}: StatChanged 호출 - StatType: {StatType}, PreviousValue: {previousValue}, CurrentValue: {currentValue}", this);
        if (Mathf.Approximately(previousValue, currentValue))
        {
            return;
        }
        UpdateRuntimeStat(StatType);

        if (StatType == StatType.MaxHp)
        {
            float increasedAmount = currentValue - previousValue;

            if (increasedAmount > 0f)
            {
                currentHealth += increasedAmount;
            }

            currentHealth = Mathf.Clamp(currentHealth, 0f, currentValue);
        }

        OnStateChanged?.Invoke(StatType, previousValue, currentValue);
        if (StatType == StatType.MaxHp)
        {
            OnHealthChanged?.Invoke(currentHealth, currentValue);
        }
    }
    public void AddExperience(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        int previousLevel = playerLevel.CurrentLevel;

        playerLevel.AddExperience(amount);

        UpdateRuntimeLevel();

        OnExperienceChanged?.Invoke(playerLevel.CurrentExperience, playerLevel.RequiredExperience);

        if (previousLevel != playerLevel.CurrentLevel)
        {
            OnLevelChanged?.Invoke(playerLevel.CurrentLevel);
            UpgradeManager.Instance.CreateUpgradeChoices();
        }
    }
    public int GetRequiredExperience()
    {
        return playerLevel.RequiredExperience;
    }

    //경험치 얻는 걸 이벤트로 변경시
    //private void HandleExperience(int amount)
    //{
    //    if (amount <= 0)
    //    {
    //        return;
    //    }

    //    int previousLevel = playerLevel.CurrentLevel;

    //    int levelUpCount = playerLevel.AddExperience(amount);

    //    UpdateRuntimeLevel();

    //    //for (int i = 0; i < levelUpCount; i++)
    //    //{
    //    //    int newLevel = previousLevel + i + 1;

    //    //    HandleLevelUp(newLevel);
    //    //}
    //}



    private void RuntimeStat()
    {
        runtimeStats.Clear();

        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            if (statType == StatType.None)
            {
                continue;
            }
            RuntimeStatEntry runtimeStat = new RuntimeStatEntry(
                statType,
                playerStats.GetBaseStat(statType),
                playerStats.GetItemStat(statType),
                playerStats.GetTotalStat(statType));

            runtimeStats.Add(runtimeStat);
        }
    }

    private void UpdateRuntimeStat(StatType statType)
    {
        foreach (RuntimeStatEntry runtimeStat in runtimeStats)
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


    public void TakeDamage(float damage)
    {
        if (damage <= 0f)
        {
            return;
        }

        float maxHp = GetStat(StatType.MaxHp);

        float newHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHp);

        if (Mathf.Approximately(currentHealth, newHealth))
        {
            return;
        }

        currentHealth = newHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHp);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f)
        {
            return;
        }

        float maxHp = GetStat(StatType.MaxHp);

        float newHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHp);

        if (Mathf.Approximately(currentHealth, newHealth))
        {
            return;
        }

        currentHealth = newHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHp);
    }
    private void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;

        OnDied?.Invoke();
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