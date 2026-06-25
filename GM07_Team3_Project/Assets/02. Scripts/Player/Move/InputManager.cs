using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    [Header("마우스 설정")]
    [SerializeField] private float lookSensitivity = 0.1f;

    [SerializeField] private bool invertXAxis;
    [SerializeField] private bool invertYAxis;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction slideAction;
    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");

        lookAction = InputSystem.actions.FindAction("Player/Look");

        jumpAction = InputSystem.actions.FindAction("Player/Jump");

        slideAction = InputSystem.actions.FindAction("Player/Slide");

        if (moveAction == null)
        {
            Debug.LogError("Player/Move 액션을 찾지 못했습니다.", this);
        }

        if (lookAction == null)
        {
            Debug.LogError("Player/Look 액션을 찾지 못했습니다.", this);
        }

        if (jumpAction == null)
        {
            Debug.LogError("Player/Jump 액션을 찾지 못했습니다.", this);
        }

        if (slideAction == null)
        {
            Debug.LogError("Player/Slide 액션을 찾지 못했습니다.", this);
        }
    }
    private void OnEnable()
    {
        moveAction?.Enable();
        lookAction?.Enable();
        jumpAction?.Enable();
        slideAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        lookAction?.Disable();
        jumpAction?.Disable();
        slideAction?.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    public Vector3 GetMoveInput()
    {
        if (!CanProcessInput() || moveAction == null)
        {
            return Vector3.zero;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 moveInput = new Vector3(input.x, 0f, input.y);

        return moveInput.normalized;
        //return Vector3.ClampMagnitude(moveInput, 1f);
    }

    public Vector2 GetLookInput()
    {
        if (!CanProcessInput() || lookAction == null)
        {
            return Vector2.zero;
        }

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        if (invertXAxis)
        {
            lookInput.x *= -1f;
        }
        if (invertYAxis)
        {
            lookInput.y *= -1f;
        }

        return lookInput * lookSensitivity;
    }

    public bool GetJumpInputDown()
    {
        if (!CanProcessInput() || jumpAction == null)
        {
            return false;
        }

        return jumpAction.WasPressedThisFrame();
    }
    public bool GetSlideInput()
    {
        if (!CanProcessInput() || slideAction == null)
        {
            return false;
        }

        return slideAction.IsPressed();
    }
}
