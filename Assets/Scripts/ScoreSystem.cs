using UnityEngine;

public class ScoreSystem
{
    public int CalculateScore(int healthLeft, int healthMax)
    {
        float ratio = (float)healthLeft / healthMax;
        return Mathf.RoundToInt(ratio * 1000);
    }

    public int GetStarRating(int healthLeft, int healthMax)
    {
        float ratio = (float)healthLeft / healthMax;

        if (ratio >= 0.8f)
        {
            return 3;

        }
        else if (ratio >= 0.5f)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    public int CalculateReward(int healthLeft, int healthMax)
    {
        float ratio = (float)healthLeft / healthMax;
        int baseReward = 100;
        int reward = Mathf.RoundToInt(baseReward * ratio * 10);
        return Mathf.Max(reward, baseReward);
    }
}
