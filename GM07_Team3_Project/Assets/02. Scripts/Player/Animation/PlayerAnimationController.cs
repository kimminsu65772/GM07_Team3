using UnityEngine;

[DefaultExecutionOrder(100)]
public sealed class PlayerAnimationController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Animator animator;

    [SerializeField]
    private PlayerCharacterController movementController;

    [SerializeField]
    private PlayerStatController statController;

    private static readonly int MoveHash = Animator.StringToHash("Move");

    private static readonly int FallingHash = Animator.StringToHash("Falling");

    private static readonly int JumpHash = Animator.StringToHash("Jump");

    private static readonly int DieHash = Animator.StringToHash("Die");

    private bool isDead;

    private AnimatorUpdateMode originalUpdateMode;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (movementController == null)
        {
            movementController = GetComponent<PlayerCharacterController>();
        }

        if (statController == null)
        {
            statController = GetComponent<PlayerStatController>();
        }

        if (animator == null)
        {
            Debug.LogError($"{name}: Animator가 없습니다.", this);
            enabled = false;
            return;
        }

        if (movementController == null)
        {
            Debug.LogError($"{name}: PlayerCharacterController가 없습니다.", this);
            enabled = false;
            return;
        }

        if (statController == null)
        {
            Debug.LogError($"{name}: PlayerStatController가 없습니다.", this);
            enabled = false;
            return;
        }

        originalUpdateMode = animator.updateMode;
    }

    private void OnEnable()
    {
        if (statController != null)
        {
            statController.OnDied += HandleDied;
        }
    }

    private void OnDisable()
    {
        if (statController != null)
        {
            statController.OnDied -= HandleDied;
        }

        originalUpdateMode = animator.updateMode;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        Vector3 velocity = movementController.Velocity;

        float horizontalSpeedSqr = velocity.x * velocity.x + velocity.z * velocity.z;

        bool isMoving = horizontalSpeedSqr > 0.0001f;

        bool isFalling = movementController.CurrentSurfaceState == PlayerSurfaceState.Airborne && velocity.y <= 0f;

        animator.SetBool(MoveHash, isMoving);

        animator.SetBool(FallingHash, isFalling);

        if (movementController.JumpedThisFrame)
        {
            Jump();
        }
    }
    private void Jump()
    {
        animator.SetTrigger(JumpHash);
    }

    public void HandleDied()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        animator.SetBool(MoveHash, false);

        animator.SetBool(FallingHash, false);

        animator.ResetTrigger(JumpHash);

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        animator.SetTrigger(DieHash);

        movementController.enabled = false;
    }
}