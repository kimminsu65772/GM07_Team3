using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEventManager : MonoBehaviour
{
    //UI에 보내는 카드 후보 리시트 
    public static event Action<List<UpgradeData>> OnUpgradeChoicesCreated;

    //업그레이드 선택  이벤트 
    //선택한 카드 1개를 보내는 이벤트
    public static event Action<UpgradeData> OnUpgradeSelected;


    public static void CreateUpgradeChoices(List<UpgradeData>upgrades)
    {
        OnUpgradeChoicesCreated?.Invoke(upgrades);
    }

    public static void SelectUpgrade(UpgradeData upgradeData)
    {
        OnUpgradeSelected?.Invoke(upgradeData);
    }
}
