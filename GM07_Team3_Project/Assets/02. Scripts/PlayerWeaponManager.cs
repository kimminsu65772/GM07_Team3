using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponBase weaponBase;
    [SerializeField] private UpgradeEventManager upgradeEventManager;

    private void Awake()
    {
        if (weaponBase == null)
        {
            weaponBase = GetComponent<WeaponBase>();
        }
    }

    private void OnEnable()
    {
        UpgradeEventManager.Instance. OnUpgradeSelected += OnUpgradeSelected;
    }

    private void OnDisable()
    {
        UpgradeEventManager.Instance. OnUpgradeSelected -= OnUpgradeSelected;
    }

    private void OnUpgradeSelected(UpgradeOption option)
    {
        if (option == null || option.Data == null)
        {
            return;
        }

        if (option.Data.UpgradeType != UpgradeType.Weapon)
        {
            return;
        }

        Debug.Log($"무기 획득: {option.Data.UpgradeName} / Value: {option.Value}");

        weaponBase.Init(option, transform);
    }
}