using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerTest : Singleton<InputManagerTest>
{
    public Action<bool> OnPauseStateChanged;


    private InputAction pauseAction;
    private bool isPaused;

    protected override void Awake()
    {
        base.Awake();
        if (pauseAction == null)
            pauseAction = InputSystem.actions.FindAction("Pause");
        isPaused = false;
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            Debug.Log("Pause button pressed");
            isPaused = !isPaused;
            OnPauseStateChanged?.Invoke(isPaused);
        }
    }
}
