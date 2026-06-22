using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "GameData/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    [SerializeField] private List<UpgradeData> upgrades = new List<UpgradeData>();
    
    //외부에서 참조할 프로퍼티
    public IReadOnlyList<UpgradeData> Upgrades {get { return upgrades; } }

    public List<UpgradeData>  GetRandomUpgrades(int count)
    {
        //원본 복사
        List<UpgradeData> copyList = new List<UpgradeData>(upgrades);

        //결과 리스트
        List<UpgradeData> result = new List<UpgradeData>();

        //뽑을 카드 개수
        for (int i = 0; i < count; i++)
        {
            if (copyList.Count <= 0)
            {
                break;
            }
            //랜덤한 능력 or 무기 뽑기
            int randomIndex = Random.Range(0, copyList.Count);

            //결과 넣고
            result.Add(copyList[randomIndex]);
            //중복방지
            copyList.RemoveAt(randomIndex);
        }
        //결과 반환
        return result;
    }
        
    
}
