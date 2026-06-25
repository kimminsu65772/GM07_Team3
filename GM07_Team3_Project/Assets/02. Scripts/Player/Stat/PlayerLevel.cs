using System;

public sealed class PlayerLevel
{
    private readonly int maxLevel;
    private readonly int[] requiredExperiences;
    public int CurrentLevel { get; private set; }
    public int CurrentExperience { get; private set; }
    public int RequiredExperience
    {
        get
        {
            if (CurrentLevel >= maxLevel)
            {
                return 0;
            }

            int index = CurrentLevel - 1;

            return Math.Max(1, requiredExperiences[index]);
        }
    }
    public PlayerLevel(PlayerLevelSO playerLevelData)
    {
        if (playerLevelData == null)
        {
            throw new ArgumentNullException(nameof(playerLevelData));
        }

        maxLevel = playerLevelData.MaxLevel;

        //requiredExperiences = playerLevelData.RequiredExperiences;
        requiredExperiences = (int[])playerLevelData.RequiredExperiences.Clone();

        CurrentLevel = 1;
        CurrentExperience = 0;
    }

    public int AddExperience(int amount)
    {
        if (amount <= 0)
        {
            return 0;
        }

        if (CurrentLevel >= maxLevel)
        {
            return 0;
        }

        CurrentExperience += amount;

        int levelUpCount = 0;

        while (CurrentLevel < maxLevel)
        {
            int index = CurrentLevel - 1;

            int requiredExperience = Math.Max(1, requiredExperiences[index]);

            if (CurrentExperience < requiredExperience)
            {
                break;
            }

            CurrentExperience -= requiredExperience;

            CurrentLevel++;
            levelUpCount++;
        }

        if (CurrentLevel >= maxLevel)
        {
            CurrentExperience = 0;
        }

        return levelUpCount;
    }
}
