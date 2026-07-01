using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Game/Audio/Sound Database")]
public sealed class SoundDatabaseSO : ScriptableObject
{
    [SerializeField] private List<SoundData> sounds = new();

    public IReadOnlyList<SoundData> Sounds => sounds;
}