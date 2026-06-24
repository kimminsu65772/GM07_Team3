using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevelSO", menuName = "Game/Level/Player Level SO")]
public sealed class PlayerLevelSO : ScriptableObject
{
    [Header("레벨별 필요 경험치")]
    [SerializeField] private int[] requiredExperiences = new int[0];
    public int MaxLevel => requiredExperiences.Length + 1;
    public int[] RequiredExperiences => requiredExperiences;
}
