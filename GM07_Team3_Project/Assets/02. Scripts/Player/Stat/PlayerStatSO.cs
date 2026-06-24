using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Game/Stats/Player Stat SO")]
public class PlayerStatSO : ScriptableObject
{
    [Header("캐릭터 정보")]
    [SerializeField]
    private string characterName;

    [Header("기본 스탯")]
    [SerializeField]
    private List<StatEntry> baseStats = new();

    public string CharacterName => characterName;
    public IReadOnlyList<StatEntry> BaseStats => baseStats;
}

[Serializable]
public sealed class StatEntry
{
    [SerializeField]
    private StateType stateType;

    [SerializeField]
    private float value;

    public StateType StateType => stateType;
    public float Value => value;
}