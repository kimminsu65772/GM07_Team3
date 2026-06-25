using UnityEngine;

/*
 * UIRoot
 * UIRoot 클래스는 게임 내 UI 요소들의 최상위 부모 역할을 수행합니다.
 * UIRoot에는 각 UI를 관리하는 UIController에 대한 참조가 포함되어 있으며,
 * UIManager는 UIRoot를 통해 각 UIController에게 UI 제어 요청을 호출 할 수 있습니다.
 */

public class UIRoot : MonoBehaviour
{
    [Header("UI Controller Setting")]
    [SerializeField] private PauseUIController pauseUIController;
    [SerializeField] private UpgradeUIController upgradeUIController;

    // UI 매니저에서 참조하기 위한 프로퍼티
    public PauseUIController PauseUIController => pauseUIController; // = get { return pauseUIController; }
    public UpgradeUIController UpgradeUIController => upgradeUIController;

    private void Awake()
    {
        UIManager.Instance.RegisterUIRoot(this);
    }

    // 씬을 넘어가서 UIRoot가 파괴될 때 UIManager에서 참조를 해제
    private void OnDestroy()
    {
        if (!UIManager.HasInstance) return;
        UIManager.Instance.UnregisterUIRoot(this);
    }
}
