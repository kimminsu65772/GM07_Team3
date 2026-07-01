using System.Collections;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [Header("Camera Setting")]
    [SerializeField] private GameOverCamera gameOverCamera;
    [SerializeField] private Camera gamePlayCamera;

    [Header("Game Over Popup")]
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private float popupDelay = 1.0f;

    private WaitForSecondsRealtime wfs;

    private Coroutine gameOverRoutine;
    private int originalGamePlayCameraCullingMask;
    private bool isShowing;

    private void Awake()
    {
        if (gamePlayCamera != null)
        {
            originalGamePlayCameraCullingMask = gamePlayCamera.cullingMask;
        }

        wfs = new WaitForSecondsRealtime(popupDelay);
    }

    public void ShowGameOver()
    {
        if (isShowing) return;
        isShowing = true;

        if (gameOverRoutine != null)
        {
            StopCoroutine(gameOverRoutine);
        }

        if (gamePlayCamera != null)
        {
            originalGamePlayCameraCullingMask = gamePlayCamera.cullingMask;
        }
        gameOverCamera.PlayGameOverCamera(gamePlayCamera);
        // 카메라를 추적하는 카메라가 플레이어 마스크를 무시하도록 하여 플레이어가 2명이 보이는 문제를 해결
        if (gamePlayCamera != null)
        {
            gamePlayCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
        }
        gameOverPopup.gameObject.SetActive(true);
        gameOverPopup.CloseGameOverPopup();
        gameOverRoutine = StartCoroutine(ShowGameOverRoutine());
    }

    public void HideGameOver()
    {
        isShowing = false;

        if (gameOverRoutine != null)
        {
            StopCoroutine(gameOverRoutine);
            gameOverRoutine = null;
        }
        gameOverCamera?.StopCamera();
        gameOverPopup?.CloseGameOverPopup();
        ResetGamePlayCamera();
    }

    public void OnClickRetryBtn()
    {
        HideGameOver();
        gameOverPopup?.gameObject.SetActive(false);
        UIManager.Instance.HandleGameOverMenuRequest(GameOverMenuType.Retry);
    }

    public void OnClickMainMenuBtn()
    {
        HideGameOver();
        gameOverPopup?.gameObject.SetActive(false);
        UIManager.Instance.HandleGameOverMenuRequest(GameOverMenuType.MainMenu);
    }

    private void OnDisable()
    {
        ResetGamePlayCamera();
    }

    private void ResetGamePlayCamera()
    {
        if (gamePlayCamera == null) return;

        gamePlayCamera.cullingMask = originalGamePlayCameraCullingMask;
    }

    private IEnumerator ShowGameOverRoutine()
    {
        yield return wfs;
        gameOverPopup.OpenGameOverPopup();
        gameOverRoutine = null;
    }
}
