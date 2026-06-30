using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "GameData/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    [SerializeField] private List<UpgradeData> upgrades = new List<UpgradeData>();

    [Header("레어도 출현 확률")]
    [SerializeField] private float commonChance = 70.0f;
    [SerializeField] private float uncommonChance = 25.0f;
    [SerializeField] private float rareChance = 5.0f;

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
                rarity = GetRandomRarity();
                finalValue = GetRandomValueByRarity(seletedData, rarity);
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

    ////레어도 구현
    ////코드 수정을 위해 잠시 주석처리
    //private UpgradeRarity GetRarityByValue(UpgradeData data, float value)
    //{
    //    if (data.UseRandomValue == false)
    //    {
    //        return UpgradeRarity.Common;
    //    }

    //    if (Mathf.Approximately(data.MinValue, data.MaxValue))
    //    {
    //        return UpgradeRarity.Common;
    //    }

    //    //Mathf.InverseLerp란? 
    //    //아이템 마다 최소,최대 스텟이 다를 때 정수로 딱 나누어 떨어지지 않을 수 있기 때문에
    //    // 비율로 값을 나눠주는 InverseLerp를 사용
    //    float normalizedValue = Mathf.InverseLerp(data.MinValue, data.MaxValue, value);

    //    if (normalizedValue <= 0.3f)
    //    {
    //        return UpgradeRarity.Common;
    //    }
    //    else if (normalizedValue <= 0.6f)
    //    {
    //        return UpgradeRarity.Uncommon;
    //    }
    //    else
    //    {
    //        return UpgradeRarity.Rare;
    //    }
    //}

    //레어도에 따른 등장 확률을 나눠보자
    private UpgradeRarity GetRandomRarity()
    {
        float totalChance = commonChance + uncommonChance + rareChance;
        float randomValue = Random.Range(0.0f, totalChance);
        //1부터 100까지의 구간 확인
        if (randomValue < commonChance)
        {
            //일반 구간이면 일반
            return UpgradeRarity.Common;
        }
        else if (randomValue < commonChance + uncommonChance)
        {
            //그 이상 레어 이하면 희귀
            return UpgradeRarity.Uncommon;
        }
        else
        {
            //아니면 레어
            return UpgradeRarity.Rare;
        }
    }

    //레어도에 따라서 랜덤 수치가 나올 구간을 정해주자
    private float GetRandomValueByRarity(UpgradeData data, UpgradeRarity rarity)
    {
        //범위 부터 정해주기
        float min = data.MinValue;
        float max = data.MaxValue;
        //최대 최소값도 구해
        float range = max - min;
        //설정이 이상하면 min값 반환하고 끝
        if (range <= 0.0f)
        {
            return min;
        }
        //뽑게될 랜덤 구간
        float valueMin;
        float valueMax;

        switch (rarity)
        {
            //일반 구간 계산
            case UpgradeRarity.Common:
                valueMin = min;
                valueMax = min + range * 0.3f;
                break;
              //희귀 구간 계산
            case UpgradeRarity.Uncommon:
                valueMin = min + range * 0.3f;
                valueMax = min + range * 0.6f;
                break;
               //레어 구간 계산
            case UpgradeRarity.Rare:
                valueMin = min + range * 0.6f;
                valueMax = max;
                break;
            //처리 방식이 애매하면 그냥 일반 아이템으로 ㄱㄱ
            default:
                valueMin = min;
                valueMax = min + range * 0.3f;
                break;
        }

        return Random.Range(valueMin, valueMax);
    }
}
