using System.Collections.Generic;
using UnityEngine;

public class UpgradeOptionTest : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;

    private void OnEnable()
    {
        UpgradeEventManager.OnUpgradeChoicesCreated += OnChoicesCreated;
        UpgradeEventManager.OnUpgradeSelected += OnUpgradeSelected;
    }

    private void OnDisable()
    {
        UpgradeEventManager.OnUpgradeChoicesCreated -= OnChoicesCreated;
        UpgradeEventManager.OnUpgradeSelected -= OnUpgradeSelected;
    }

    private void Start()
    {
        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager가 연결되지 않았습니다.");
            return;
        }

        upgradeManager.CreateUpgradeChoices();
    }

    private void OnChoicesCreated(List<UpgradeOption> options)
    {
        Debug.Log("=== 업그레이드 후보 생성 ===");

        for (int i = 0; i < options.Count; i++)
        {
            UpgradeOption option = options[i];

            Debug.Log(
                $"후보 {i + 1} / 이름: {option.Data.UpgradeName} / 랜덤 Value: {option.Value}"
            );
        }

        if (options.Count > 0)
        {
            Debug.Log("테스트용으로 첫 번째 카드 자동 선택");
            UpgradeEventManager.SelectUpgrade(options[0]);
        }
    }

    private void OnUpgradeSelected(UpgradeOption option)
    {
        Debug.Log(
            $"선택됨 / 이름: {option.Data.UpgradeName} / 적용 Value: {option.Value}"
        );
    }
}