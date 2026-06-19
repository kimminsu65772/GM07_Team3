using UnityEngine;

/*
 * UIManager
 * UImanager 클래스는 게임 내 UI 요소들로 부터 이벤트를 받아 다른 매니저나 시스템에 전달하는 역할을 수행합니다.
 */
public class UIManager : Singleton<UIManager>
{
    private UIRoot currentUIRoot;
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

    ////////////////////////////
    /// Pause Menu 관련 메서드
    ////////////////////////////

    // InputManager에서 일시정지 버튼이 누른 것을 감지하면 UIManager에서 실행할 메서드.
    public void TogglePausePanel()
    {
        TimeManagerTest.Instance.ToggleTimeScale();
        currentUIRoot.PauseUIController.TogglePausePanel();
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
}
