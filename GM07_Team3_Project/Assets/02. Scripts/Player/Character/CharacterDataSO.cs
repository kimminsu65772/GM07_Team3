using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Game/Character Data")]
public sealed class CharacterDataSO : ScriptableObject
{
    [Header("캐릭터 정보")]
    [SerializeField] private string characterName;
    [SerializeField] private Sprite icon;

    [TextArea]
    [SerializeField] private string description;

    [Header("캐릭터 모델")]
    [SerializeField] private GameObject modelPrefab;

    [Header("기본 스탯")]
    [SerializeField] private PlayerStatSO playerStatData;

    public string CharacterName => characterName;
    public Sprite Icon => icon;
    public string Description => description;
    public GameObject ModelPrefab => modelPrefab;
    public PlayerStatSO PlayerStatData => playerStatData;
}