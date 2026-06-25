using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private WeaponBase weaponBase;

    private void Awake()
    {
        weaponBase = GetComponent<WeaponBase>();
    }

    private void OnEnable()
    {
        UpgradeEventManager.OnUpgradeSelected += ApplyUpgrade;
    }

    private void OnDisable()
    {
        UpgradeEventManager.OnUpgradeSelected -= ApplyUpgrade;
    }

    private void ApplyUpgrade(UpgradeOption option)
    {
        if (option == null || option.Data == null)
        {
            Debug.LogError("UpgradeOption 또는 UpgradeData가 비어 있습니다.");
            return;
        }

        if (option.Data.UpgradeType != UpgradeType.Weapon)
        {
            return;
        }

        if (weaponBase == null)
        {
            Debug.LogError("Player에 WeaponBase가 없습니다.");
            return;
        }

        if (option.Data.BulletPrefab == null)
        {
            Debug.LogError($"{option.Data.UpgradeName}의 WeaponAttackPrefab이 비어 있습니다.");
            return;
        }

        weaponBase.Init(option, transform);

        Debug.Log($"무기 데이터 적용 완료: {option.Data.UpgradeName}, Value: {option.Value}");
    }
}