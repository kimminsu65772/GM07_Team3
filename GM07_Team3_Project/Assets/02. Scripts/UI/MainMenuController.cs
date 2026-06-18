using System;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] MenuGroup menuGroup;
    private MainMenuType currentMenuType;

    // 다른 매니저에서 메인 메뉴 컨트롤러 객체를 생성할 필요 없이 바로 이벤트를 구독할 수 있도록 static으로 선언
    public static Action<SceneType> OnRequireToLoadGameScene;
    public static UnityAction RequireToOpenOptionPanel;
    public static UnityAction RequireToQuitGame;
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
        Debug.Log($"메뉴 선택됨: {currentMenuType}");
        switch (currentMenuType)
        {
            case MainMenuType.GameStart:
                Debug.Log("로드 씬 요청");
                OnRequireToLoadGameScene?.Invoke(SceneType.GameScene);
                break;
            case MainMenuType.Option:
                Debug.Log("옵션 패널 열기 요청");
                break;
            case MainMenuType.Exit:
                Debug.Log("게임 매니저에게 게임 종료를 요청");
                RequireToQuitGame?.Invoke();
                break;
        }
    }

    private void OnDestroy()
    {
        if (menuGroup == null) return;
        menuGroup.OnMenuSelected -= RequestAction;
    }

    // 아래는 메인 메뉴 컨트롤러의 이벤트를 구독/구독해제 하는 메서드들을 정의한 부분입니다.

    // 옵션 패널을 여는 이벤트를 구독/구독해제 하는 메서드
    public static void SubscribeToOpenOptionPanel(UnityAction action)
    {
        RequireToOpenOptionPanel += action;
    }

    public static void UnsubscribeToOpenOptionPanel(UnityAction action)
    {
        RequireToOpenOptionPanel -= action;
    }

    // 게임 종료를 요청하는 이벤트를 구독/구독해제 하는 메서드
    public static void SubscribeToQuitGame(UnityAction action)
    {
        RequireToQuitGame += action;
    }

    public static void UnsubscribeToQuitGame(UnityAction action)
    {
        RequireToQuitGame -= action;
    }
}
