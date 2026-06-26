using DG.Tweening;
using System.Collections;
using UnityEngine;

/*
 * MenuGroup
 * MenuGroup는 메뉴 UI 요소들을 그룹화하고 해당 버튼들의 등장 애니메이션을 관리합니다.
 */
public class MenuGroup : MonoBehaviour
{
    // 메뉴 UI 요소들을 배열로 관리
    [SerializeField] private MenuUI[] menuButtons;
    [SerializeField] private CanvasGroup canvasGroup;

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
            // 클로저 문제 해결을 위해 인덱스를 별도의 변수에 저장
            int index = i; 
            sequence.AppendCallback(() => menuButtons[index].ActiveMenuButton());
            sequence.AppendInterval(0.1f);
        }
        yield return sequence.WaitForCompletion();

        // 모든 메뉴 버튼이 등장한 후, 메뉴를 선택 가능 상태로 변경
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].SetIsClickable(true);
        }
        canvasGroup.interactable = true;
    }

    public void SelectMenu(MenuUI menu)
    {
        // 메뉴가 선택되면 더 이상 다른 메뉴를 선택할 수 없도록 설정
        canvasGroup.interactable = false;
        menu.PlaySelectTween();

        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].SetIsClickable(false);
        }
    }
}
