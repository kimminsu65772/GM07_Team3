using UnityEngine;

public sealed class ThirdPersonCameraController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform pitchPivot;
    [SerializeField] private InputManager inputHandler;

    [Header("위아래 회전 제한")]
    [SerializeField] private float minPitch = -25f;
    [SerializeField] private float maxPitch = 65f;

    [Header("카메라 따라가기")]
    [SerializeField] private float followSharpness = 20f;

    private float yaw;
    private float pitch;

    private void Awake()
    {
        if (cameraTarget == null)
        {
            Debug.LogError(
                $"{name}: CameraTarget이 등록되지 않았습니다.",
                this);
        }

        if (pitchPivot == null)
        {
            Debug.LogError(
                $"{name}: PitchPivot이 등록되지 않았습니다.",
                this);
        }

        if (inputHandler == null)
        {
            Debug.LogError(
                $"{name}: PlayerInputHandler가 등록되지 않았습니다.",
                this);
        }

        yaw = transform.eulerAngles.y;

        if (pitchPivot != null)
        {
            pitch =
                NormalizeAngle(
                    pitchPivot.localEulerAngles.x);
        }
    }

    private void LateUpdate()
    {
        FollowTarget();
        RotateCamera();
    }

    private void FollowTarget()
    {
        if (cameraTarget == null)
        {
            return;
        }

        /*
         * 플레이어를 따라가되 약간 부드럽게 이동한다.
         */
        float followRatio =
            1f - Mathf.Exp(
                -followSharpness * Time.deltaTime);

        transform.position =
            Vector3.Lerp(
                transform.position,
                cameraTarget.position,
                followRatio);
    }

    private void RotateCamera()
    {
        if (inputHandler == null ||
            pitchPivot == null)
        {
            return;
        }

        Vector2 lookInput =
            inputHandler.GetLookInput();

        yaw += lookInput.x;

        /*
         * 마우스를 위로 움직일 때
         * 카메라가 위를 보도록 y를 뺀다.
         */
        pitch -= lookInput.y;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch);

        /*
         * CameraRig 루트:
         * 좌우 회전
         */
        transform.rotation =
            Quaternion.Euler(
                0f,
                yaw,
                0f);

        /*
         * PitchPivot:
         * 위아래 회전
         */
        pitchPivot.localRotation =
            Quaternion.Euler(
                pitch,
                0f,
                0f);
    }

    private static float NormalizeAngle(
        float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }
}