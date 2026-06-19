using UnityEngine;
using DG.Tweening;
using System;

/*
 * PausePanel
 * PausePanel 클래스는 인풋 매니저에서 이벤트를 받아 게임을 일시정지시키고 패널을 활성화/비활성화하는 기능을 담당합니다.
 */
public class PausePanel : MonoBehaviour
{
    [Header("Slide Duration")]
    [SerializeField] private float slideDuration = 0.1f;
    [Header("패널 슬라이드 거리")]
    [SerializeField] private float slideDistance = 750f;

    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Buttons")]
    [SerializeField] private PauseMenuUI[] buttons;


    private bool isPaused = false;
    private RectTransform rectTransform;
    private float originalXPosition;

    InputManagerTest inputManager;

    public event Action<PauseMenuType> OnPauseMenuSelected;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalXPosition = rectTransform.anchoredPosition.x;
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
    }
    private void OnEnable()
    {
        inputManager = InputManagerTest.Instance;
        // 이중 구독 방지를 위해 먼저 이벤트 제거 시도 후 구독
        inputManager.OnPauseStateChanged -= TogglePauseState;
        inputManager.OnPauseStateChanged += TogglePauseState;
    }

    private void OnDisable()
    {
        inputManager.OnPauseStateChanged -= TogglePauseState;
    }

    private void TogglePauseState(bool state)
    {
        Debug.Log($"Pause state changed: {state}");
        isPaused = state;
        if (isPaused)
        {
            // 게임 일시정지
            Time.timeScale = 0f;
            // 패널을 활성화하는 동시에 DOTween 애니메이션이 재생하는 메서드 실행
            PlaySlideOpen();
        }
        else
        {
            PlaySlideClose();
        }
    }

    // DOTween 애니메이션을 재생하여 오른쪽에서 왼쪽으로 슬라이드 하는 메서드
    private void PlaySlideOpen()
    {
        if (!isPaused) return;
        rectTransform.DOKill();

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // SetUpdate(true)를 사용하여 Time.timeScale이 0일 때도 애니메이션이 재생되도록 설정
        rectTransform
            .DOAnchorPosX(originalXPosition - slideDistance, slideDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                subscribeButtonAction();
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
    }

    private void PlaySlideClose()
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
                unsubscribeButtonAction();
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
            });
    }

    private void subscribeButtonAction()
    {
        foreach (var button in buttons)
        {
            button.OnPauseMenuClicked += HandleButtonClick;
        }
    }

    private void unsubscribeButtonAction()
    {
        foreach (var button in buttons)
        {
            button.OnPauseMenuClicked -= HandleButtonClick;
        }
    }

    private void HandleButtonClick(PauseMenuType buttonType)
    {
        Debug.Log($"Pause menu button clicked: {buttonType}");
        OnPauseMenuSelected?.Invoke(buttonType);
    }
}
