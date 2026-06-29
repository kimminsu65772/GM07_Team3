using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * UIManager
 * UImanager 클래스는 게임 내 UI 요소들로 부터 이벤트를 받아 다른 매니저나 시스템에 전달하는 역할을 수행합니다.
 */
public class UIManager : Singleton<UIManager>
{
    private UIRoot currentUIRoot;
    private UIPanelType currentPanel = UIPanelType.None;

    // 카드가 선택되면 UpgradeEventManager에 전달할 이벤트
    public Action<UpgradeOption> onUpgradeSelected;
    public Action onPausePressed;

    private void OnEnable()
    {
        // UpgradeEventManager의 업그레이드 선택 이벤트에 onUpgradeSelected를 구독
        UpgradeEventManager.Instance.OnUpgradeChoicesCreated -= HandleUpgradeChoiceCreated;
        UpgradeEventManager.Instance.OnUpgradeChoicesCreated += HandleUpgradeChoiceCreated;
    }

    private void OnDisable()
    {
        if (!UpgradeEventManager.HasInstance) return;
        UpgradeEventManager.Instance.OnUpgradeChoicesCreated -= HandleUpgradeChoiceCreated;
    }
    public void HandleMainMenuRequest(MainMenuType mainMenuType)
    {
        Debug.Log($"메인 메뉴 요청: {mainMenuType}");
        switch(mainMenuType)
        {
            case MainMenuType.GameStart:
                GameSceneManager.Instance.LoadScene(SceneType.GameScene);
                break;
            case MainMenuType.Option:
                // 옵션 메뉴 요청 처리
                Debug.Log("옵션 메뉴 요청 처리");
                break;
            case MainMenuType.Exit:
                // 게임 종료 요청 처리
                // 실제로는 게임 매니저를 통해 종료 요청을 전달해야 함.
                Application.Quit();
                break;
        }
    }

    // UIManager는 버튼이 클릭되는 순간에 생성되어 게임이 종료될 때까지 씬 전역을 돌아다니며 존재하게 됨.
    // 이 경우 다른 씬에 있는 UI 요소들을 알 수 없기 때문에, UIRoot라는 오브젝트를 통해서 해당 씬에 존재하는 모든 UI 요소들을 찾아서 관리할 수 있게 함.
    public void RegisterUIRoot(UIRoot uiRoot)
    {
        Debug.Log($"UIRoot 등록: {uiRoot.name}");
        currentUIRoot = uiRoot;
    }

    public void UnregisterUIRoot(UIRoot uiRoot)
    {
        Debug.Log($"UIRoot 해제: {uiRoot.name}");
        if (currentUIRoot == uiRoot)
        {
            currentUIRoot = null;
        }
    }

    ////////////////////////////
    /// Pause Menu 관련 메서드
    ////////////////////////////

    // InputManager에서 일시정지 버튼이 누른 것을 감지하면 UIManager에서 일시정지 패널을 요청하는 메서드
    public void TogglePausePanel()
    {
        if (currentUIRoot == null) return;

        if (!CanControlPanel(UIPanelType.Pause)) return;

        onPausePressed?.Invoke();

        if (AlreadyOpenPanel(UIPanelType.Pause))
        {
            currentUIRoot.PauseUIController.ClosePausePanel();
            currentPanel = UIPanelType.None;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            currentUIRoot.PauseUIController.OpenPausePanel();
            currentPanel = UIPanelType.Pause;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HandlePauseMenuRequest(PauseMenuType pauseMenuType)
    {
        Debug.Log($"일시정지 메뉴 요청: {pauseMenuType}");
        switch(pauseMenuType)
        {
            case PauseMenuType.Resume:
                // 게임 재개 요청 처리
                // UI -> 게임 매니저 -> TimeManager 순으로 요청을 전달할지
                // UI -> TimeManager 순으로 요청을 전달할지 고민 필요.
                Debug.Log("게임 재개 요청 처리");
                TimeManagerTest.Instance.ToggleTimeScale();
                break;
            case PauseMenuType.Quit:
                // 메인 메뉴로 나가기 요청 처리
                GameSceneManager.Instance.LoadScene(SceneType.MainMenu);
                break;
        }
    }

    ////////////////////////////
    /// Upgrade UI 관련 메서드
    ////////////////////////////

    // UpgradeEventManager에서 업그레이드 선택 이벤트가 발생하면 호출되는 메서드
    // 컨트롤러에게 레벨업 패널을 열고 UpgradeData를 업그레이드 UI에 전달하도록 요청.
    // [임시] 일단은 레벨업 패널이 열릴 때 게임이 일시정지되도록 구현.
    // 추후에는 레벨업 이벤트를 TimeManager에서 받아서 게임이 일시정지되도록 구현하도록 변경 필요.

    private void HandleUpgradeChoiceCreated(List<UpgradeOption> upgradeCards)
    {
        Debug.Log("업그레이드 뽑기 이벤트 발생");
        if (currentUIRoot == null) return;
        currentPanel = UIPanelType.LevelUp;
        TimeManagerTest.Instance.ToggleTimeScale();
        currentUIRoot.UpgradeUIController.ShowLevelUpPanel(upgradeCards);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HandleUpgradeSelected(UpgradeOption UpgradeData)
    {
        Debug.Log($"업그레이드 선택 이벤트 발생: {UpgradeData.Data.UpgradeName}");
        currentPanel = UIPanelType.None;
        onUpgradeSelected?.Invoke(UpgradeData);
        TimeManagerTest.Instance.ToggleTimeScale();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 제어하고자 하는 패널이 제어 가능 상태인지 확인하는 메서드
    private bool CanControlPanel(UIPanelType panelType)
    {
        // 이미 열려있는 경우에는 다시 닫을 수 있도록 같은 타입의 패널
        if (currentPanel == UIPanelType.None || currentPanel == panelType)
        {
            return true;
        }
        else
        {
            Debug.LogWarning($"이미 다른 패널이 열려있습니다: {currentPanel}");
            return false;
        }
    }

    private bool AlreadyOpenPanel(UIPanelType panelType)
    {
        return currentPanel == panelType;
    }

    ////////////////////////////
    /// Game Over UI 관련 메서드
    ////////////////////////////
    
    public void HandleGameOver()
    {
        Debug.Log("게임 오버 이벤트 발생");
        if (currentUIRoot == null) return;
        // 게임오버 카메라 활성화

        currentPanel = UIPanelType.GameOver;
        currentUIRoot.GameOverController.ShowGameOver();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
