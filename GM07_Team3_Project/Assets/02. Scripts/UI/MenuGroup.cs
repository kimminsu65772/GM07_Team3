using DG.Tweening;
using System.Collections;
using UnityEngine;

/*
 * MenuGroup
 * MenuGroup는 메뉴 UI 요소들을 그룹화하는 클래스입니다.
 * 이 클래스에서는 메뉴 버튼 UI 요소들의 등장을 관리합니다.
 */
public class MenuGroup : MonoBehaviour
{
    // 메뉴 UI 요소들을 배열로 관리
    [SerializeField] private MenuUI[] menuButtons;
    [SerializeField] private CanvasGroup canvasGroup;

    private MainMenuType selectedMenu = MainMenuType.None;

    private void Start()
    {
        // 게임이 시작될 때 메뉴 UI 요소들을 서서히 등장시키는 코루틴을 시작
        StartCoroutine(FadeInMenusCo());
    }

    // 게임이 시작되면 메뉴 UI 요소들을 서서히 등장시키는 코루틴을 시작
    IEnumerator FadeInMenusCo()
    {
        if (!canvasGroup) yield break;
        if (menuButtons == null || menuButtons.Length == 0) yield break;

        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].DeactiveMenuButton();
        }

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < menuButtons.Length; i++)
        {
            int index = i; // 클로저 문제 해결을 위해 인덱스를 별도의 변수에 저장
            sequence.AppendCallback(() => menuButtons[index].ActiveMenuButton());
            sequence.AppendInterval(0.1f); // 각 메뉴 버튼이 등장하는 간격
        }
        yield return sequence.WaitForCompletion();

        // 모든 메뉴 버튼이 등장한 후, 메뉴를 선택 가능 상태로 변경
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].SetIsClickable(true);
        }
        canvasGroup.interactable = true;
    }
}
