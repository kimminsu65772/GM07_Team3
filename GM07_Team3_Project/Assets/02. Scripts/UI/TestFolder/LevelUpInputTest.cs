using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelUpInputTest : MonoBehaviour
{
    private InputAction levelUpAction;

    public static Action OnLevelUp;

    private void Awake()
    {
        levelUpAction = new InputAction(
            name: "LevelUpTest",
            type: InputActionType.Button,
            binding: "<Keyboard>/l");
    }

    private void OnEnable()
    {
        levelUpAction.Enable();
    }

    private void OnDisable()
    {
        levelUpAction.Disable();
    }

    private void Update()
    {
        if (levelUpAction.triggered)
        {
            Debug.Log("Level Up Input Triggered");
            OnLevelUp?.Invoke();
        }
    }
}
