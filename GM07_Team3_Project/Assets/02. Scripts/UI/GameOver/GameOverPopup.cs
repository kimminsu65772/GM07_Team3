using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image gameOverText;
    [SerializeField] private MenuUI[] gameOverButtons;

    [Header("Fade Setting")]
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Result UI Setting")]
    [SerializeField] private float textFadeDuration = 0.25f;
    [SerializeField] private float buttonStartDelay = 0.1f;
    [SerializeField] private float buttonInterval = 0.1f;

    private Sequence resultUISequence;
    private bool isOpened;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void OpenGameOverPopup()
    {
        if (canvasGroup == null) return;

        isOpened = true;
        canvasGroup.DOKill();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;
        DeactiveResultUI();

        canvasGroup
            .DOFade(1f, fadeDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                if (!isOpened) return;

                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                ActiveResultUI();
            });
    }

    public void CloseGameOverPopup()
    {

        isOpened = false;
        canvasGroup.DOKill();
        DeactiveResultUI();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OnDisable()
    {
        if (canvasGroup == null) return;

        canvasGroup.DOKill();
        DeactiveResultUI();
    }

    private void OnDestroy()
    {
        if (canvasGroup == null) return;

        canvasGroup.DOKill();
        DeactiveResultUI();
    }

    private void ActiveResultUI()
    {
        if (gameOverText != null)
        {
            gameOverText.DOKill();
            gameOverText.DOFade(1f, textFadeDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);
        }

        resultUISequence?.Kill();
        resultUISequence = DOTween.Sequence().SetUpdate(true);
        resultUISequence.AppendInterval(buttonStartDelay);

        if (gameOverButtons == null) return;

        for (int i = 0; i < gameOverButtons.Length; i++)
        {
            if (gameOverButtons[i] == null) continue;

            int buttonIndex = i;
            resultUISequence.AppendCallback(() =>
            {
                gameOverButtons[buttonIndex].ActiveMenuButton();
                gameOverButtons[buttonIndex].SetIsClickable(true);
            });
            resultUISequence.AppendInterval(buttonInterval);
        }
    }

    private void DeactiveResultUI()
    {
        resultUISequence?.Kill();

        if (gameOverText != null)
        {
            gameOverText.DOKill();
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 0f);
        }

        if (gameOverButtons == null) return;

        for (int i = 0; i < gameOverButtons.Length; i++)
        {
            if (gameOverButtons[i] == null) continue;

            gameOverButtons[i].DeactiveMenuButton();
        }
    }
}
