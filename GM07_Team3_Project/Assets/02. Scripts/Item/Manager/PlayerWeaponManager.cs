using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header("시작 무기 ")]
    [SerializeField] private UpgradeData startWeaponData;

    [Header("무기 관리 부모")]
    [SerializeField] private Transform weaponContainer;

    private readonly List<WeaponBase> equippedWeapons = new List<WeaponBase>();

    private void Awake()
    {
        CreateWeaponContainer();
    }
    private void Start()
    {
        EquipStartWeapon();
    }

    private void OnEnable()
    {
        if (UpgradeEventManager.Instance != null)
        {
            UpgradeEventManager.Instance.OnUpgradeSelected += HandleUpgradeSelected;
        }
    }

    private void OnDisable()
    {
        if (UpgradeEventManager.Instance != null)
        {
            UpgradeEventManager.Instance.OnUpgradeSelected -= HandleUpgradeSelected;
        }
    }

    private void CreateWeaponContainer()
    {
        if (weaponContainer != null)
        {
            return;
        }

        GameObject containerObj = new GameObject("WeaponContainer");
        containerObj.transform.SetParent(transform);
        containerObj.transform.localPosition = Vector3.zero;
        containerObj.transform.localRotation = Quaternion.identity;

        weaponContainer = containerObj.transform;
    }

    private void EquipStartWeapon()
    {
        if (startWeaponData == null)
        {
            Debug.LogWarning("시작 무기가 설정되지 않았습니다.");
            return;
        }

        if (startWeaponData.UpgradeType != UpgradeType.Weapon)
        {
            Debug.LogError("시작 무기 데이터가 Weapon 타입이 아닙니다.");
            return;
        }

        UpgradeOption option = new UpgradeOption(startWeaponData, startWeaponData.Value);

        AddWeapon(option);
    }

    private void HandleUpgradeSelected(UpgradeOption option)
    {
        if (option == null || option.Data == null)
        {
            return;
        }

        // 무기 카드가 아니면 무시
        if (option.Data.UpgradeType != UpgradeType.Weapon)
        {
            return;
        }
        AddWeapon(option);
    }

    private void AddWeapon(UpgradeOption option)
    {
        GameObject weaponObj = new GameObject($"Weapon_{option.Data.UpgradeName}");

        weaponObj.transform.SetParent(weaponContainer);
        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;

        WeaponBase weaponBase = weaponObj.AddComponent<WeaponBase>();

        weaponBase.Init(option, transform);

        equippedWeapons.Add(weaponBase);
    }

}
