using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string profileID;

    public string playerName = "Player";
    public int coins = 1000;
    public int totalScore = 0;
    public string avatarBase64 = "";
    public string currentBalloonID;
    public List<string> unlockedBalloonIDs;
    public List<LevelProgress> levelProgressData;

    public PlayerData()
    {
        profileID = Guid.NewGuid().GetHashCode().ToString();

        playerName = "Player";
        coins = 1000;
        totalScore = 0;
        avatarBase64 = "";

        currentBalloonID = "0";
        unlockedBalloonIDs = new List<string> { currentBalloonID };

        levelProgressData = new List<LevelProgress>();
        levelProgressData.Add(new LevelProgress(0, true));
    }
}