using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    

    [Header("무기 관리 부모")]
    [SerializeField] private Transform weaponContainer;

    private readonly List<WeaponBase> equippedWeapons = new List<WeaponBase>();

    // 이미 장착한 무기 데이터 목록 저장
    private readonly List<UpgradeData> equippedWeaponDatas = new List<UpgradeData>();
    private UpgradeData startWeaponData;

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
        CharacterDataSO selectedCharacter = CharacterSelection.SelectedCharacter;

        if (selectedCharacter == null || selectedCharacter.StartItemData == null)
        {
            return;
        }

        if (selectedCharacter.StartItemData.UpgradeType != UpgradeType.Weapon)
        {
            return;
        }

        startWeaponData = selectedCharacter.StartItemData;

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
        if (option == null || option.Data == null)
        {
            return;
        }

        // 구버전 -> 이미 같은 무기를 가지고 있으면 중복 장착 방지
        // 신버전 -> 같은 무기를 골랐을 때 보너스 데미지를 추가 해줌 
        if (equippedWeaponDatas.Contains(option.Data))
        {
            ItemStatManager itemStatManager = GetComponent<ItemStatManager>();

            if (itemStatManager != null)
            {
                itemStatManager.AddDamagePercentBonus(0.1f);
            }

            return;
        }


        GameObject weaponObj;

        if (option.Data.WeaponPrefab != null)
        {
            weaponObj = Instantiate(option.Data.WeaponPrefab, weaponContainer);
            weaponObj.name = $"Weapon-{option.Data.UpgradeName}";
        }
        else
        {
            weaponObj = new GameObject($"Weapon-{option.Data.UpgradeName}");
            weaponObj.transform.SetParent(weaponContainer);
        }

        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;

        WeaponBase weaponBase = weaponObj.GetComponent<WeaponBase>();

        if (weaponBase == null)
        {
            weaponBase = weaponObj.AddComponent<WeaponBase>();
        }

        weaponBase.Init(option, transform);

        equippedWeapons.Add(weaponBase);

        equippedWeaponDatas.Add(option.Data);
    }
}
