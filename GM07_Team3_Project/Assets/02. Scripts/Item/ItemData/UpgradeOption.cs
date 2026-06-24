using UnityEngine;

public class UpgradeOption
{

  public UpgradeData Data { get; private set; }
    public float Value { get; private set; }

    public UpgradeOption(UpgradeData data, float value)
    {
        Data = data;
        Value = value;
    }



}
