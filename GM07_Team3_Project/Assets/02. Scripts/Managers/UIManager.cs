using UnityEngine;

/*
 * UIManager
 * UImanager 클래스는 게임 내 UI 요소들로 부터 이벤트를 받아 다른 매니저나 시스템에 전달하는 역할을 수행합니다.
 */
public class UIManager : Singleton<UIManager>
{
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
}
