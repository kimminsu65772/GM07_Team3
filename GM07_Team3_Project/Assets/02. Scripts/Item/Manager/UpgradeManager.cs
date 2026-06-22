using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private UpgradeDatabase upgradeDatabase;
    [SerializeField] private int choiceCount = 3;

    public void CreateUpgradeChoices()
    {
        if (upgradeDatabase == null)
        {
            Debug.LogError("UpgradeDatabase가 연결되지 않았습니다.");
            return;
        }

        List<UpgradeData> result = upgradeDatabase.GetRandomUpgrades(choiceCount);

        UpgradeEventManager.CreateUpgradeChoices(result);
    }
}