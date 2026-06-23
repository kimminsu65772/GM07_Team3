using UnityEngine;

public class UpgradeOption : MonoBehaviour
{

  public UpgradeData Data { get; private set; }
    public float Value { get; private set; }

    public UpgradeOption(UpgradeData data, float value)
    {
        Data = data;
        Value = value;
    }



}
