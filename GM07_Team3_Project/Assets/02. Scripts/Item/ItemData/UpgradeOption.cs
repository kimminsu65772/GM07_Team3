using UnityEngine;

public class UpgradeOption
{

  public UpgradeData Data { get; private set; }
    public float Value { get; private set; }
    public UpgradeRarity Rarity { get; private set; }

    //레어도에 기본 레어값을 설정해서 다른 쪽에서 오류방지
    public UpgradeOption(UpgradeData data, float value, UpgradeRarity rarity = UpgradeRarity.Common)
    {
        Data = data;
        Value = value;
        Rarity = rarity;
    }



}
