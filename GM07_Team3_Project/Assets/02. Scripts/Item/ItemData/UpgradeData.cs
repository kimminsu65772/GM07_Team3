using UnityEngine;

[CreateAssetMenu(fileName = " UpgradeData", menuName ="GameData/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("업그레이드 타입")]
    [SerializeField] private UpgradeType upgradeType;

    [Header("능력치 타입")]
    [SerializeField] private StateType stateType;

    [Header("이름")]
    [SerializeField] private string upgradeName;

    [Header("설명")]
    [TextArea]
    [SerializeField] private string description;

    [Header("아이콘")]
    [SerializeField] private Sprite icon;

    [Header("무기 프리펩")]
    [SerializeField] private GameObject weaponPrefab;

    [Header("무기 투사체 프리펩")]
    [SerializeField] private GameObject weaponAttackPrefab;

    [Header("수치 값")]
    [SerializeField] private float value;

    [Header("랜덤한 수치 사용 여부")]
    [SerializeField] private bool useRandomValue;

    [Header("랜덤한 수치 범위")]
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;
     




    //외부에서 사용 가능한 읽기전용 프로퍼티
    public UpgradeType UpgradeType {  get { return upgradeType; }}
    public StateType StateType { get {return stateType;} }
    public string UpgradeName { get {return upgradeName;} }
    public string Description { get {return description;} } 
    public Sprite Icon { get {return icon;} }
    public GameObject WeaponPrefab { get {return weaponPrefab;} }
    public GameObject BulletPrefab { get { return weaponAttackPrefab; } }
    public float Value { get {return value;} }  
    public bool UseRandomValue { get {return useRandomValue;}}
    public float MinValue { get {return minValue;} }
    public float MaxValue { get { return maxValue; } }

}
