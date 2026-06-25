using UnityEngine;

public class UpgradeTestRunner : MonoBehaviour
{
    [SerializeField] private UpgradeData testWeaponData;
    [SerializeField] private UpgradeData testWeaponData2;

    private void Start()
    {
        if (testWeaponData == null || testWeaponData2 == null)
        {
            Debug.LogError("테스트할 UpgradeData가 비어 있습니다.");
            return;
        }

        float finalValue = testWeaponData.Value;
        float finalValue2 = testWeaponData2.Value;
        if (testWeaponData.UseRandomValue)
        {
            finalValue = Random.Range(testWeaponData.MinValue, testWeaponData.MaxValue);
        }
        if (testWeaponData2.UseRandomValue)
        {
            finalValue = Random.Range(testWeaponData2.MinValue, testWeaponData2.MaxValue);
        }

        UpgradeOption option = new UpgradeOption(testWeaponData, finalValue);
        UpgradeOption option2 = new UpgradeOption(testWeaponData2, finalValue);

        UpgradeEventManager.SelectUpgrade(option);
    }
}