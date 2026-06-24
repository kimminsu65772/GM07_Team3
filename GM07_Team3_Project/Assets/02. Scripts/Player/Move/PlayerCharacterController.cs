using UnityEngine;

public enum PlayerSurfaceState
{
    None = -1,
    WalkGround,
    SteepSlope,
    Airborne,
    Length
}


public sealed class PlayerCharacterController : MonoBehaviour
{
    private const float DirectionEpsilon = 0.001f;

    [Header("참조")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GroundChecker groundChecker;

    [Header("일반 이동")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("점프와 중력")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -25f;

    [SerializeField] private float groundVerticalVelocity = -2f;

    [SerializeField] private float jumpGroundIgnoreTime = 0.15f;

    [Header("가파른 경사")]
    [SerializeField] private float steepSlopeAcceleration = 20f;
    [SerializeField] private float maxSteepSlopeSpeed = 12f;
    [SerializeField] private float steepControlAcceleration = 8f;

    [Header("보행 가능 지면")]
    [SerializeField] private float minGroundSnapDistance = 0.01f;
    [SerializeField] private float maxGroundSnapDistance = 0.25f;

    [Header("런타임 상태 확인")]
    [SerializeField] private PlayerSurfaceState currentSurfaceState = PlayerSurfaceState.Airborne;

    [SerializeField] private Vector3 steepSlideVelocity;

    private CharacterController characterController;
    private InputManager inputManager;

    private float verticalVelocity;
    private float groundIgnoreTimer;

    public bool IsGrounded { get; private set; }

    public Vector3 Velocity { get; private set; }

    public PlayerSurfaceState CurrentSurfaceState => currentSurfaceState;
    private float MaxWalkSlopeAngle => characterController.slopeLimit;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        inputManager = GetComponent<InputManager>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (groundChecker == null)
        {
            Debug.LogError($"{name}: 자식 GroundCheck의 groundChecker가 연결되지 않았습니다.", this);
            enabled = false;
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogWarning($"{name}: Camera Transform이 없습니다. ", this);
        }
    }

    private void Update()
    {
        GroundIgnoreTimer();

        SurfaceState();

        Vector3 moveInput = inputManager.GetMoveInput();

        Vector3 moveDirection = CalculateCameraRelativeDirection(moveInput);

        TryJump();

        Velocity = CalculateVelocity(moveDirection);

        CollisionFlags collisionFlags = characterController.Move(Velocity * Time.deltaTime);

        HandleCollisionFlags(collisionFlags);

        SurfaceState();

        if (TrySnapToWalkableGround())
        {
            SurfaceState();
        }

        RotateCharacter(moveDirection);
    }

    private void GroundIgnoreTimer()
    {
        if (groundIgnoreTimer <= 0f)
        {
            return;
        }
        groundIgnoreTimer = Mathf.Max(0f, groundIgnoreTimer - Time.deltaTime);
    }

    private void SurfaceState()
    {
        groundChecker.GroundCheck();
        UpdateSurfaceState();
    }

    private void UpdateSurfaceState()
    {
        if (groundIgnoreTimer > 0f)
        {
            currentSurfaceState = PlayerSurfaceState.Airborne;
            IsGrounded = false;
            return;
        }

        if (!groundChecker.hasGround)
        {
            currentSurfaceState = PlayerSurfaceState.Airborne;

            IsGrounded = false;
            return;
        }

        if (groundChecker.groundAngle <= MaxWalkSlopeAngle)
        {
            currentSurfaceState = PlayerSurfaceState.WalkGround;

            IsGrounded = characterController.isGrounded;

            return;
        }

        currentSurfaceState = PlayerSurfaceState.SteepSlope;

        IsGrounded = false;
    }

    private void TryJump()
    {
        if (!IsGrounded || !inputManager.GetJumpInputDown())
        {
            return;
        }

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        groundIgnoreTimer = jumpGroundIgnoreTime;

        currentSurfaceState = PlayerSurfaceState.Airborne;

        IsGrounded = false;

        steepSlideVelocity = Vector3.zero;
    }

    private Vector3 CalculateVelocity(Vector3 moveDirection)
    {
        switch (currentSurfaceState)
        {
            case PlayerSurfaceState.WalkGround:
                return CalculateWalkGroundVelocity(moveDirection);

            case PlayerSurfaceState.SteepSlope:
                return CalculateSteepSlopeVelocity(moveDirection);

            default:
                return CalculateAirborneVelocity(moveDirection);
        }
    }

    private Vector3 CalculateWalkGroundVelocity(Vector3 moveDirection)
    {
        steepSlideVelocity = Vector3.zero;

        bool hasMoveInput = moveDirection.sqrMagnitude > DirectionEpsilon * DirectionEpsilon;

        if (!hasMoveInput)
        {
            verticalVelocity = groundVerticalVelocity;

            return Vector3.up * verticalVelocity;
        }

        Vector3 walkMoveDirection = Vector3.ProjectOnPlane(moveDirection, groundChecker.groundNormal);

        if (walkMoveDirection.sqrMagnitude <= DirectionEpsilon * DirectionEpsilon)
        {
            verticalVelocity = groundVerticalVelocity;

            return Vector3.up * verticalVelocity;
        }

        walkMoveDirection.Normalize();
        verticalVelocity = 0f;

        return walkMoveDirection * moveSpeed;
    }

    private Vector3 CalculateAirborneVelocity(Vector3 moveDirection)
    {
        steepSlideVelocity = Vector3.zero;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 horizontalVelocity = moveDirection * moveSpeed;

        return horizontalVelocity + Vector3.up * verticalVelocity;
    }

    private Vector3 CalculateSteepSlopeVelocity(Vector3 moveDirection)
    {
        verticalVelocity = 0f;

        Vector3 groundNormal = groundChecker.groundNormal;

        Vector3 steepSlopeDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormal);

        if (steepSlopeDirection.sqrMagnitude <= DirectionEpsilon * DirectionEpsilon)
        {
            steepSlideVelocity = Vector3.zero;

            return Vector3.zero;
        }

        steepSlopeDirection.Normalize();

        Vector3 Acceleration = Vector3.ProjectOnPlane(Vector3.down * steepSlopeAcceleration, groundNormal);

        steepSlideVelocity += Acceleration * Time.deltaTime;

        Vector3 Input = Vector3.ProjectOnPlane(moveDirection, groundNormal);

        Vector3 uphillDirection = -steepSlopeDirection;

        float uphillInputAmount = Vector3.Dot(Input, uphillDirection);

        if (uphillInputAmount > 0f)
        {
            Input -= uphillDirection * uphillInputAmount;
        }

        if (Input.sqrMagnitude > DirectionEpsilon * DirectionEpsilon)
        {
            Input.Normalize();

            steepSlideVelocity += Input * steepControlAcceleration * Time.deltaTime;
        }

        steepSlideVelocity = Vector3.ProjectOnPlane(steepSlideVelocity, groundNormal);

        float uphillSpeed = Vector3.Dot(steepSlideVelocity, uphillDirection);

        if (uphillSpeed > 0f)
        {
            steepSlideVelocity -= uphillDirection * uphillSpeed;
        }

        steepSlideVelocity = Vector3.ClampMagnitude(steepSlideVelocity, maxSteepSlopeSpeed);

        return steepSlideVelocity;
    }

    private void HandleCollisionFlags(CollisionFlags collisionFlags)
    {
        bool hitCeiling = (collisionFlags & CollisionFlags.Above) != 0;

        if (hitCeiling && verticalVelocity > 0f)
        {
            verticalVelocity = 0f;
        }
    }

    private bool TrySnapToWalkableGround()
    {
        if (groundIgnoreTimer > 0f || verticalVelocity > 0f)
        {
            return false;
        }

        if (currentSurfaceState != PlayerSurfaceState.WalkGround)
        {
            return false;
        }

        if (characterController.isGrounded || !groundChecker.hasGround)
        {
            return false;
        }

        if (groundChecker.groundAngle > MaxWalkSlopeAngle)
        {
            return false;
        }

        float snapDistance = groundChecker.groundDistance;

        if (snapDistance <= minGroundSnapDistance || snapDistance > maxGroundSnapDistance)
        {
            return false;
        }

        characterController.Move(Vector3.down * snapDistance);

        verticalVelocity = groundVerticalVelocity;

        return true;
    }

    private Vector3 CalculateCameraRelativeDirection(Vector3 moveInput)
    {
        if (moveInput.sqrMagnitude <= DirectionEpsilon * DirectionEpsilon)
        {
            return Vector3.zero;
        }

        if (cameraTransform == null)
        {
            return moveInput;
        }

        Quaternion cameraYawRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);

        Vector3 cameraForward = cameraYawRotation * Vector3.forward;

        Vector3 cameraRight = cameraYawRotation * Vector3.right;

        Vector3 worldMoveDirection = cameraForward * moveInput.z + cameraRight * moveInput.x;

        worldMoveDirection.y = 0f;

        return worldMoveDirection;
    }

    private void RotateCharacter(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude <= DirectionEpsilon * DirectionEpsilon)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
