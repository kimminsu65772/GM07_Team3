using UnityEngine;
using UnityEngine.InputSystem;

public class LevelUpInputTest : MonoBehaviour
{
    private InputAction levelUpAction;
    private UpgradeManager upgradeManager;

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
        if (!levelUpAction.WasPressedThisFrame()) return;

        if (upgradeManager == null)
        {
            upgradeManager = FindFirstObjectByType<UpgradeManager>();
        }

        if (upgradeManager == null)
        {
            Debug.LogWarning("UpgradeManager was not found. Level up test event was skipped.");
            return;
        }

        upgradeManager.CreateUpgradeChoices();
        Debug.Log("Level up test event triggered.");
    }
}
