using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private UpgradeDatabase upgradeDatabase;
    [SerializeField] private int choiceCount = 3;

  

    public void CreateUpgradeChoices()
    {
        if (upgradeDatabase == null) return;
     

        List<UpgradeOption> result = upgradeDatabase.GetRandomUpgrades(choiceCount);

        // result 리스트 이벤트로 전송
        UpgradeEventManager.Instance.CreateUpgradeChoices(result);
    }

    private void OnDisable()
    {
        LevelUpInputTest.OnLevelUp -= CreateUpgradeChoices;
    }
}