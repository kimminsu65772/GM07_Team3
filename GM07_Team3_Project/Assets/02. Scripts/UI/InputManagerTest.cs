using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerTest : Singleton<InputManagerTest>
{
    private InputAction pauseAction;

    protected override void Awake()
    {
        base.Awake();
        if (pauseAction == null)
        {
            pauseAction = new InputAction(
                name: "PauseTest",
                type: InputActionType.Button,
                binding: "<Keyboard>/p");
        }
    }

    private void OnEnable()
    {
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    private void Update()
    {
        if (pauseAction.triggered)
        {
            UIManager.Instance.TogglePausePanel();
        }
    }
}
