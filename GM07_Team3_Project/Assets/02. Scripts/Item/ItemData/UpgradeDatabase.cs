using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "GameData/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    [SerializeField] private List<UpgradeData> upgrades = new List<UpgradeData>();
    
    //외부에서 참조할 프로퍼티
    public IReadOnlyList<UpgradeData> Upgrades {get { return upgrades; } }

    public List<UpgradeOption>  GetRandomUpgrades(int count)
    {
        //원본 복사
        List<UpgradeData> copyList = new List<UpgradeData>(upgrades);

        //랜덤 값의 결과 리스트
        List<UpgradeOption> result = new List<UpgradeOption>();

        //뽑을 카드 개수
        for (int i = 0; i < count; i++)
        {
            if (copyList.Count <= 0)
            {
                break;
            }
            //랜덤한 능력 or 무기 뽑기
            int randomIndex = Random.Range(0, copyList.Count);
            UpgradeData seletedData = copyList[randomIndex];

            float finalValue = seletedData.Value;
            //레어도 추가
            UpgradeRarity rarity = UpgradeRarity.Common;

            if(seletedData.UseRandomValue)
            {
                finalValue = Random.Range(seletedData.MinValue, seletedData.MaxValue);
                rarity = GetRarityByValue(seletedData, finalValue);
            }

            UpgradeOption option = new UpgradeOption(seletedData, finalValue, rarity);


            //결과 넣고
            result.Add(option);
            //중복방지
            copyList.RemoveAt(randomIndex);
        }
        //결과 반환
        return result;
    }

    //레어도 구현
    private UpgradeRarity GetRarityByValue(UpgradeData data, float value)
    {
        if (data.UseRandomValue == false)
        {
            return UpgradeRarity.Common;
        }

        if (Mathf.Approximately(data.MinValue, data.MaxValue))
        {
            return UpgradeRarity.Common;
        }

        //Mathf.InverseLerp란? 
        //아이템 마다 최소,최대 스텟이 다를 때 정수로 딱 나누어 떨어지지 않을 수 있기 때문에
        // 비율로 값을 나눠주는 InverseLerp를 사용
        float normalizedValue = Mathf.InverseLerp(data.MinValue, data.MaxValue, value);

        if (normalizedValue <= 0.3f)
        {
            return UpgradeRarity.Common;
        }
        else if (normalizedValue <= 0.6f)
        {
            return UpgradeRarity.Uncommon;
        }
        else
        {
            return UpgradeRarity.Rare;
        }
    }


}
