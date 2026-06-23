using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private UpgradeDatabase upgradeDatabase;
    [SerializeField] private int choiceCount = 3;

    private void OnEnable()
    {
        // 먼저 중복으로 구독괴는 것을 방지하기 위해 이벤트 구독을 해제 시도
        LevelUpInputTest.OnLevelUp -= CreateUpgradeChoices;
        LevelUpInputTest.OnLevelUp += CreateUpgradeChoices;
    }

    public void CreateUpgradeChoices()
    {
        Debug.Log("OnLevelUp event received. Creating upgrade choices...");
        if (upgradeDatabase == null) return;
     

        List<UpgradeData> result = upgradeDatabase.GetRandomUpgrades(choiceCount);

        // result 리스트 이벤트로 전송
        UpgradeEventManager.Instance.CreateUpgradeChoices(result);
    }

    private void OnDisable()
    {
        LevelUpInputTest.OnLevelUp -= CreateUpgradeChoices;
    }
}