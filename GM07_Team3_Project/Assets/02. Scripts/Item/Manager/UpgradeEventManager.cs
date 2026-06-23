using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEventManager : Singleton<UpgradeEventManager>
{
    // UI에 보내는 카드 후보 리시트 
    public event Action<List<UpgradeData>> OnUpgradeChoicesCreated;

    // 업그레이드 선택  이벤트 
    // 선택한 카드 1개를 보내는 이벤트
    public event Action<UpgradeData> OnUpgradeSelected;

    private void OnEnable()
    {
        // UI매니저의 업그레이드 선택 이벤트에 SelectUpgrade 메서드를 구독
        UIManager.Instance.onUpgradeSelected -= SelectUpgrade;
        UIManager.Instance.onUpgradeSelected += SelectUpgrade;
    }

    public void CreateUpgradeChoices(List<UpgradeData>upgrades)
    {
        Debug.Log("UpgradeEventManager: CreateUpgradeChoices called with " + upgrades.Count + " upgrades.");
        OnUpgradeChoicesCreated?.Invoke(upgrades);
    }

    public void SelectUpgrade(UpgradeData upgradeData)
    {
        OnUpgradeSelected?.Invoke(upgradeData);
    }

    private void OnDisable()
    {
        if (!UIManager.HasInstance) return;
        UIManager.Instance.onUpgradeSelected -= SelectUpgrade;
    }
}
