using UnityEngine;

public sealed class ThirdPersonCameraController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform pitchPivot;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputManager inputHandler;

    [Header("위아래 회전 제한")]
    [SerializeField] private float minPitch = -25f;
    [SerializeField] private float maxPitch = 65f;

    [Header("카메라 따라가기")]
    [SerializeField] private float followSpeed = 20f;

    [Header("카메라 충돌 레이어")]
    [SerializeField] private LayerMask cameraCollisionLayer;

    [Header("카메라 레이 반지름")]
    [SerializeField] private float cameraCollisionRadius = 0.25f;

    [Header("카메라 충돌 여유 지점")]
    [SerializeField] private float collisionOffset = 0.05f;

    [Header("카메라 충돌 최소 지점")]
    [SerializeField] private float minimumCameraDistance = 0.3f;

    [Header("카메라의 기존 포지션으로 가는 값")]
    [SerializeField] private float cameraReturnSpeed = 10f;

    private float yaw;
    private float pitch;

    private Vector3 cameraLocalPosition;

    private void Awake()
    {
        if (cameraTarget == null)
        {
            Debug.LogError($"{name}: CameraTarget이 등록되지 않았습니다.", this);
        }

        if (pitchPivot == null)
        {
            Debug.LogError($"{name}: PitchPivot이 등록되지 않았습니다.", this);
        }

        if (cameraTransform == null)
        {
            Debug.LogError($"{name}: Main Camera Transform이 등록되지 않았습니다.", this);
        }
        else
        {
            cameraLocalPosition = cameraTransform.localPosition;
        }

        if (inputHandler == null)
        {
            Debug.LogError($"{name}: InputManager가 등록되지 않았습니다.", this);
        }

        yaw = transform.eulerAngles.y;

        if (pitchPivot != null)
        {
            pitch = NormalizeAngle(pitchPivot.localEulerAngles.x);
        }
    }

    private void LateUpdate()
    {
        FollowTarget();
        RotateCamera();
        CameraCollision();
    }

    private void FollowTarget()
    {
        if (cameraTarget == null)
        {
            return;
        }

        float followRatio = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, followRatio);
    }

    private void RotateCamera()
    {
        if (inputHandler == null || pitchPivot == null)
        {
            return;
        }

        Vector2 lookInput = inputHandler.GetLookInput();

        yaw += lookInput.x;
        pitch -= lookInput.y;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        pitchPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void CameraCollision()
    {
        if (pitchPivot == null ||
            cameraTransform == null)
        {
            return;
        }

        Vector3 previousCameraPosition = pitchPivot.TransformPoint(cameraLocalPosition);

        Vector3 castVector = previousCameraPosition - pitchPivot.position;
        float Distance = castVector.magnitude;

        if (Distance <= Mathf.Epsilon)
        {
            return;
        }

        Vector3 castDirection = castVector / Distance;

        bool hasCast =
            Physics.SphereCast(
                pitchPivot.position,
                cameraCollisionRadius,
                castDirection,
                out RaycastHit hit,
                Distance,
                cameraCollisionLayer,
                QueryTriggerInteraction.Ignore);

        if (hasCast)
        {
            float collisionDistance = Mathf.Clamp(hit.distance - collisionOffset, minimumCameraDistance, Distance);

            cameraTransform.position = pitchPivot.position + castDirection * collisionDistance;

            return;
        }
        float returnRatio = 1f - Mathf.Exp(-cameraReturnSpeed * Time.deltaTime);

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, previousCameraPosition, returnRatio);
    }

    private static float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }

    private void OnDrawGizmosSelected()
    {
        if (pitchPivot == null ||
            cameraTransform == null)
        {
            return;
        }

        Vector3 desiredPosition;

        if (Application.isPlaying)
        {
            desiredPosition = pitchPivot.TransformPoint(cameraLocalPosition);
        }
        else
        {
            desiredPosition = cameraTransform.position;
        }

        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(pitchPivot.position, desiredPosition);

        Gizmos.DrawWireSphere(desiredPosition, cameraCollisionRadius);
    }
}
