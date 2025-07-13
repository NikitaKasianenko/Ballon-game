[System.Serializable]
public class LevelProgress
{
    public int levelIndex;
    public bool isUnlocked;
    public int stars;
    public int bestScore;

    public LevelProgress(int index, bool unlocked = false, int starCount = 0, int score = 0)
    {
        levelIndex = index;
        isUnlocked = unlocked;
        stars = starCount;
        bestScore = score;
    }
}