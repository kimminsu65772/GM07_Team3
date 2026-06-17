using System;
using UnityEngine;

public class UpgradeEventManager : MonoBehaviour
{
    //업그레이드 선택  이벤트 
    public static event Action<UpgradeData> OnUpgradeSelected;

    public static void SelectUpgrade(UpgradeData upgradeData)
    {
        OnUpgradeSelected?.Invoke(upgradeData);
    }
}
