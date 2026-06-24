using System.Collections.Generic;
using UnityEngine;

/*
 * UpgradePanelController
 * UpgradePanelController 클래스는 업그레이드 패널 UI를 제어하고 선택된 업그레이드 옵션을 UIManager에 전달하는 역할을 수행합니다.
 */

public class UpgradeUIController : MonoBehaviour
{
    [Header("제어 대상 설정")]
    [SerializeField] private LevelUpPanel levelUpPanel;

    // UI매니저에서 레벨업 이벤트를 받으면 레벨업 패널을 활성화함.
    // 그 후 업그레이드 카드들을 레벨업 패널에 전달하여 UI로 표시할 수 있도록 함.
    private void Start()
    {
        levelUpPanel.gameObject.SetActive(false);
    }
    public void ShowLevelUpPanel(List<UpgradeOption> upgradeCards)
    {
        levelUpPanel.gameObject.SetActive(true);

        // 레벨업 패널의 등장 애니메이션 수행
        levelUpPanel.OpenLevelUpPanel(upgradeCards);
    }

    // 버튼으로부터 선택된 업그레이드 옵션을 UIManager에 전달하고 레벨업 패널을 비활성화함.
    public void UpgradeCardSelected(UpgradeOption upgradeData)
    {
        UIManager.Instance.HandleUpgradeSelected(upgradeData);
        levelUpPanel.CloseLevelUpPanel();
    }
}
