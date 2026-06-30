public static class CharacterSelection
{
    public static CharacterDataSO SelectedCharacter { get; private set; }

    public static void SelectCharacter(CharacterDataSO characterData)
    {
        SelectedCharacter = characterData;
    }

    public static void Clear()
    {
        SelectedCharacter = null;
    }
}