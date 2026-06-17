using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] MenuGroup menuGroup;
    private MainMenuType currentMenuType;

    private void Start()
    {
        if (menuGroup != null)
        {
            menuGroup.OnMenuSelected += RequestAction;
        }
    }

    private void RequestAction(MainMenuType menuType)
    {
        currentMenuType = menuType;
        
        switch(currentMenuType)
        {
            case MainMenuType.GameStart:
                Debug.Log("씬 매니저에게 로드 씬을 요청");
                break;
            case MainMenuType.Option:
                Debug.Log("옵션 패널 열기 요청");
                break;
            case MainMenuType.Exit:
                Debug.Log("게임 매니저에게 게임 종료를 요청");
                break;
        }
    }

    private void OnDestroy()
    {
        if (menuGroup == null) return;
        menuGroup.OnMenuSelected -= RequestAction;
    }
}
