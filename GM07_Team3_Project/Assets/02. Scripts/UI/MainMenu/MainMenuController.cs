using UnityEngine;

/*
 * MainMenuController
 * MainMenuController는 메인 메뉴 버튼으로부터 이벤트를 받아 UIManager에게 전달하는 역할을 수행하는 클래스입니다.
 */
public class MainMenuController : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        UIManager.Instance.HandleMainMenuRequest(MainMenuType.GameStart);
    }

    public void OnClickOptionBtn()
    {
        UIManager.Instance.HandleMainMenuRequest(MainMenuType.Option);
    }
    public void OnClickExitBtn()
    {
        UIManager.Instance.HandleMainMenuRequest(MainMenuType.Exit);
    }
}
