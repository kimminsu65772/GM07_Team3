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

    WaitForSeconds wfs;

    private Coroutine gameOverRoutine;

    private void Awake()
    {
        if (gameOverPopup != null)
        {
            gameOverPopup.CloseGameOverPopup();
        }

        wfs = new WaitForSeconds(popupDelay);
    }

    public void ShowGameOver()
    {
        if (gameOverRoutine != null)
        {
            StopCoroutine(gameOverRoutine);
        }

        gameOverCamera.PlayGameOverCamera(gamePlayCamera);
        // 카메라를 추적하는 카메라가 플레이어 마스크를 무시하도록 하여 플레이어가 2명이 보이는 문제를 해결
        gamePlayCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));

        gameOverRoutine = StartCoroutine(ShowGameOverRoutine());
    }

    public void HideGameOver()
    {
        if (gameOverRoutine != null)
        {
            StopCoroutine(gameOverRoutine);
            gameOverRoutine = null;
        }

        gameOverCamera?.StopCamera();
        gameOverPopup?.CloseGameOverPopup();
    }

    private IEnumerator ShowGameOverRoutine()
    {
        yield return wfs;

        gameOverPopup.OpenGameOverPopup();
        gameOverRoutine = null;
    }
}
