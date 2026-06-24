using UnityEngine;
using DG.Tweening;

/*
 * PausePanel
 * PausePanel 클래스는 인풋 매니저에서 이벤트를 받아 게임을 일시정지시키고 패널을 활성화/비활성화하는 기능을 담당합니다.
 */
public class PauseUIController : MonoBehaviour
{
    [Header("Slide Duration")]
    [SerializeField] private float slideDuration = 0.1f;
    [Header("패널 슬라이드 거리")]
    [SerializeField] private float slideDistance = 750f;

    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup canvasGroup;

    private bool isPaused = false;
    private RectTransform rectTransform;
    private float originalXPosition;

    private void Awake()
    {
        rectTransform = canvasGroup.GetComponent<RectTransform>();
        originalXPosition = rectTransform.anchoredPosition.x;
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    public void OpenPausePanel()
    {
        if (!isPaused)
        {
            isPaused = true;
            Open();
        }
    }

    public void ClosePausePanel()
    {
        if (isPaused)
        {
            isPaused = false;
            Close();
        }
    }

    private void Open()
    {
        if (!isPaused) return;

        // 일시정지 패널을 열기 전에 먼저 상태를 초기 상태로 만들어서 비정상적인 상태에서 열리는 것을 방지
        rectTransform.DOKill();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false);

        // 초기화 이후에 패널을 여는 동작 수행
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        rectTransform
            .DOAnchorPosX(originalXPosition - slideDistance, slideDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
    }

    private void Close()
    {
        if (isPaused) return;
        rectTransform.DOKill();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        rectTransform
            .DOAnchorPosX(rectTransform.anchoredPosition.x + slideDistance, slideDuration)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                canvasGroup.alpha = 0f;
                canvasGroup.gameObject.SetActive(false);
            });
    }

    public void OnClickResumeBtn()
    {
        ClosePausePanel();

        // 일시정지 메뉴에서 Resume 버튼이 클릭되면 UIManager에게 Resume 요청을 보내고 패널을 닫음.
        // 아마 UIManager에서는 게임 매니저에게 일시정지 해제 요청을 보낼 것으로 예상됨.
        UIManager.Instance.HandlePauseMenuRequest(PauseMenuType.Resume);
        
    }

    public void OnClickQuitBtn()
    {
        ClosePausePanel();

        // 일시정지 메뉴에서 Quit 버튼이 클릭되면 UIManager는 게임 매니저에게는 게임 종료 요청을 보내고
        // 씬 매니저에게는 메인 메뉴 씬으로 이동할 것을 요청할 것임.
        UIManager.Instance.HandlePauseMenuRequest(PauseMenuType.Quit);
    }

    private void OnDestroy()
    {
        rectTransform.DOKill();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
