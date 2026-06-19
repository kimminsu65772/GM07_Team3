using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerTest : Singleton<InputManagerTest>
{
    public Action<bool> OnPauseStateChanged;


    private InputAction pauseAction;

    protected override void Awake()
    {
        base.Awake();
        if (pauseAction == null)
            pauseAction = InputSystem.actions.FindAction("Pause");
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            UIManager.Instance.TogglePausePanel();
        }
    }
}
