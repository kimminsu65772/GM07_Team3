using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEventManager : Singleton<UpgradeEventManager>
{    //UI에 보내는 카드 후보 리시트 
    public event Action<List<UpgradeOption>> OnUpgradeChoicesCreated;

    //업그레이드 선택  이벤트 
    //선택한 카드 1개를 보내는 이벤트
    public event Action<UpgradeOption> OnUpgradeSelected;

    public void OnEnable()
    {
        UIManager.Instance.onUpgradeSelected -= SelectUpgrade;
        UIManager.Instance.onUpgradeSelected += SelectUpgrade;
    }

    public void OnDisable()
    {
        UIManager.Instance.onUpgradeSelected -= SelectUpgrade;
    }

    public void CreateUpgradeChoices(List<UpgradeOption> upgrades)
    {
        OnUpgradeChoicesCreated?.Invoke(upgrades);
        Debug.Log("업그레이드 선택 이벤트 발생");
    }

    public void SelectUpgrade(UpgradeOption upgradeOption)
    {
        OnUpgradeSelected?.Invoke(upgradeOption);
    }
}