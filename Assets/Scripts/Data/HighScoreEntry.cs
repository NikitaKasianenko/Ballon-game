[System.Serializable]

public class HighScoreEntry
{
    public string profileID;
    public string playerName;
    public int score;
    public string avatarBase64 = "";
    public HighScoreEntry(string id, string name, int score, string avatarPath)
    {
        profileID = id;
        playerName = name;
        this.score = score;
        avatarBase64 = avatarPath;
    }
}
