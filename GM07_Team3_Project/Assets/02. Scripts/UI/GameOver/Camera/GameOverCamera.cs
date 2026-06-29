using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera gameOverCamera;

    [Header("Target")]
    [SerializeField] private float targetHeight = 1f;

    [Header("Camera Movement Settings")]
    [SerializeField] private float orbitSpeed = 25f;
    [SerializeField] private float cameraHeight = 1.5f;

    [Header("Camera Zoom Settings")]
    [SerializeField] private float startDistance = 4f;
    [SerializeField] private float endDistance = 8f;
    [SerializeField] private float zoomOutDuration = 3f;

    [SerializeField] Transform player;

    private Transform target;
    private bool isPlaying;
    private float elapsedTime;
    private float currentAngle;

    private void Awake()
    {
        if (gameOverCamera == null)
        {
            gameOverCamera = GetComponent<Camera>();
        }

        // 게임 오버 전까지 카메라를 비활성화 상태로 유지
        if (gameOverCamera != null)
        {
            gameOverCamera.enabled = false;
        }
    }

    // UI 매니저에서 카메라를 활성화하도록 호출되는 메서드
    public void PlayGameOverCamera(Camera gamePlayCamera)
    {
        if (player == null || gamePlayCamera == null)
        {
            Debug.LogError("Player or Gameplay Camera is not assigned.");
            return;
        }

        target = player;
        elapsedTime = 0f;
        isPlaying = true;

        // 게임 오버 카메라를 활성화하고 시작 위치를 현재 게임 플레이 카메라의 위치로 설정
        if (gamePlayCamera != null)
        {
            gameOverCamera.transform.position = gamePlayCamera.transform.position;
            gameOverCamera.transform.rotation = gamePlayCamera.transform.rotation;
        }

        Vector3 center = GetTargetCenter();

        // 회전 시작 방향 계산
        Vector3 flatDir = gameOverCamera.transform.position - center;
        flatDir.y = 0f;

        if (flatDir.sqrMagnitude <= 0.001f)
        {
            flatDir = -player.forward; // 플레이어의 뒤쪽 방향으로 초기화
        }

        // 수평 방향 벡터를 기준으로 Y축 회전 각도를 계산
        currentAngle = Mathf.Atan2(flatDir.z, flatDir.x) * Mathf.Rad2Deg;
        gameOverCamera.enabled = true;
    }

    private Vector3 GetTargetCenter()
    {
        return target.position + Vector3.up * targetHeight;
    }

    private void LateUpdate()
    {
        if (!isPlaying || target == null) return;

        float deltaTime = Time.unscaledDeltaTime;

        elapsedTime += deltaTime;
        currentAngle += orbitSpeed * deltaTime;

        // 시간의 진행도를 0과 1 사이로 제한하여 위치를 보간
        float t = Mathf.Clamp01(elapsedTime / zoomOutDuration);
        float distance = Mathf.Lerp(startDistance, endDistance, t);

        Vector3 center = GetTargetCenter();

        Quaternion orbitRotation = Quaternion.Euler(0f, currentAngle, 0f);

        Vector3 offset = orbitRotation * Vector3.back * distance + Vector3.up * cameraHeight;

        gameOverCamera.transform.position = center + offset;
        gameOverCamera.transform.LookAt(center);
    }

    public void StopCamera()
    {
        isPlaying = false;
        target = null;
        if (gameOverCamera != null)
        {
            gameOverCamera.enabled = false;
        }
    }
}
