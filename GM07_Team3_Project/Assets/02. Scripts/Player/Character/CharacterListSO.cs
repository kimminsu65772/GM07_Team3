using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterListSO", menuName = "Game/Character List SO")]
public sealed class CharacterListSO : ScriptableObject
{
    [SerializeField]
    private List<CharacterDataSO> characters = new();

    public IReadOnlyList<CharacterDataSO> Characters => characters;
}