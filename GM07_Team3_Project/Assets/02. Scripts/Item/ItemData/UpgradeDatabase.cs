using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "GameData/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    [SerializeField] private List<UpgradeData> upgrades = new List<UpgradeData>();
    
    //외부에서 참조할 프로퍼티
    public IReadOnlyList<UpgradeData> Upgrades {get { return upgrades; } }

    public List<UpgradeData>  GetRandomUpgardes(int count)
    {
        List<UpgradeData> copyList = new List<UpgradeData>(upgrades);

        //결과 리스트
        List<UpgradeData> result = new List<UpgradeData>();

        for (int i = 0; i < count; i++)
        {
            if (copyList.Count <= 0)
            {
                break;
            }

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
