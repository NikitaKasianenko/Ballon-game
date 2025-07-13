using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public PlayerData PlayerData { get; private set; }
    private List<PlayerData> allProfiles = new List<PlayerData>();
    [SerializeField] public GameSettings gameSettings;
    [SerializeField] public SoundData soundData;

    public List<HighScoreEntry> Leaderboard { get; private set; } = new List<HighScoreEntry>();
    [SerializeField] private List<BalloonData> allBalloons = new List<BalloonData>();

    private const string ProfilesKey = "PlayerProfiles";
    private const string LastProfileIDKey = "LastProfileID";
    private const string LeaderboardKey = "Leaderboard";
    private const string GameSettingsKey = "GameSettings";


    private class ProfileList { public List<PlayerData> profiles = new List<PlayerData>(); }
    private class HighScoreList { public List<HighScoreEntry> entries = new List<HighScoreEntry>(); }

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); LoadData(); }
        else { Destroy(gameObject); }
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(ProfilesKey))
        {
            var wrapper = JsonUtility.FromJson<ProfileList>(PlayerPrefs.GetString(ProfilesKey));
            allProfiles = wrapper?.profiles ?? new List<PlayerData>();
        }

        string lastUsedID = PlayerPrefs.GetString(LastProfileIDKey, null);
        if (lastUsedID != null)
        {
            PlayerData = allProfiles.Find(p => p.profileID == lastUsedID);
        }

        if (PlayerData == null)
        {
            if (allProfiles.Count > 0)
            {
                PlayerData = allProfiles[0];
            }
            else
            {
                PlayerData = new PlayerData { playerName = "Player" };
                allProfiles.Add(PlayerData);
            }
        }

        if (PlayerPrefs.HasKey(LeaderboardKey))
        {
            var list = JsonUtility.FromJson<HighScoreList>(PlayerPrefs.GetString(LeaderboardKey));
            Leaderboard = list?.entries ?? new List<HighScoreEntry>();
        }


        if (PlayerPrefs.HasKey(GameSettingsKey))
        {
            string json = PlayerPrefs.GetString(GameSettingsKey);
            gameSettings = JsonUtility.FromJson<GameSettings>(json);
        }
        else
        {
            gameSettings = new GameSettings
            {
                soundVolume = 1f,
                musicVolume = 1f
            };
        }
    }


    public void SaveData()
    {
        if (PlayerData != null)
        {
            PlayerPrefs.SetString(LastProfileIDKey, PlayerData.profileID);
        }

        var profileWrapper = new ProfileList { profiles = allProfiles };
        string profilesJson = JsonUtility.ToJson(profileWrapper, true);
        PlayerPrefs.SetString(ProfilesKey, profilesJson);

        var leaderboardWrapper = new HighScoreList { entries = Leaderboard };
        string leaderboardJson = JsonUtility.ToJson(leaderboardWrapper, true);
        PlayerPrefs.SetString(LeaderboardKey, leaderboardJson);

        PlayerPrefs.Save();
        Debug.Log("Data saved successfully!");

        string settingsJson = JsonUtility.ToJson(gameSettings, true);
        PlayerPrefs.SetString(GameSettingsKey, settingsJson);



    }

    public void ChangeCurrentProfileName(string newName)
    {
        if (PlayerData == null || string.IsNullOrWhiteSpace(newName)) return;

        if (allProfiles.Any(p => p.playerName == newName && p.profileID != PlayerData.profileID))
        {
            Debug.LogWarning($"Name '{newName}' is already taken.");
            return;
        }

        PlayerData.playerName = newName;

        var leaderboardEntry = Leaderboard.Find(e => e.profileID == PlayerData.profileID);
        if (leaderboardEntry != null)
        {
            Debug.Log($"Updating leaderboard entry for profile {PlayerData.profileID} with new name: {newName}");
            leaderboardEntry.playerName = newName;
        }

        SaveData();
        GameEvents.UpdatePlayerName?.Invoke();
    }

    public void UpdateLevelProgress(int levelIndex, int score, int stars)
    {
        LevelProgress progress = GetLevelProgress(levelIndex);
        progress.bestScore = Mathf.Max(progress.bestScore, score);
        progress.stars = Mathf.Max(progress.stars, stars);

        PlayerData.totalScore += score;

        if (stars > 0) UnlockLevel(levelIndex + 1);

        AddOrUpdateLeaderboardEntry();
    }

    public void AddOrUpdateLeaderboardEntry()
    {
        if (PlayerData == null) return;

        var existing = Leaderboard.Find(e => e.profileID == PlayerData.profileID);

        if (existing != null)
        {
            if (PlayerData.totalScore > existing.score)
            {
                existing.score = PlayerData.totalScore;
            }
            existing.playerName = PlayerData.playerName;
            existing.avatarBase64 = PlayerData.avatarBase64;
        }
        else
        {
            Leaderboard.Add(new HighScoreEntry(PlayerData.profileID, PlayerData.playerName, PlayerData.totalScore, PlayerData.avatarBase64));
        }

        Leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        SaveData();
    }

    public void SetPlayerAvatar(string base64)
    {
        if (PlayerData == null) return;
        PlayerData.avatarBase64 = base64;

        var leaderboardEntry = Leaderboard.Find(e => e.profileID == PlayerData.profileID);
        if (leaderboardEntry != null)
        {
            leaderboardEntry.avatarBase64 = base64;
        }
        SaveData();
    }

    public BalloonData GetCurrentBalloon()
    {
        if (PlayerData == null) return allBalloons.FirstOrDefault();

        var found = allBalloons.Find(b => b.id == PlayerData.currentBalloonID);
        if (found != null) return found;

        var fallback = GetCurrentBalloon();
        if (fallback != null)
        {
            SetCurrentBalloon(fallback.id);
        }
        return fallback;
    }


    public BalloonData GetDefaultBallon()
    {
        return allBalloons.FirstOrDefault(b => b.isUnlockedByDefault) ?? allBalloons.FirstOrDefault();
    }


    public BalloonData GetBallonById(string ballonId)
    {

        var found = allBalloons.Find(b => b.id == ballonId);
        if (string.IsNullOrEmpty(ballonId) || found == null)
        {
            return GetDefaultBallon();
        }
        else
        {
            return found;
        }
    }
    public void SetCurrentBalloon(string balloonID)
    {
        if (PlayerData == null || !IsBalloonUnlocked(balloonID)) return;
        PlayerData.currentBalloonID = balloonID;
        SaveData();
    }

    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            return;
        }
        PlayerData.coins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (amount < 0)
        {
            return false;
        }
        if (PlayerData.coins >= amount)
        {
            PlayerData.coins -= amount;
            return true;
        }
        return false;
    }

    public void UnlockBalloon(string balloonID)
    {
        if (!IsBalloonUnlocked(balloonID))
        {
            PlayerData.unlockedBalloonIDs.Add(balloonID);
        }
    }

    public bool IsBalloonUnlocked(string balloonID)
    {
        return PlayerData.unlockedBalloonIDs.Contains(balloonID);
    }


    public LevelProgress GetLevelProgress(int levelIndex)
    {
        LevelProgress progress = PlayerData.levelProgressData.Find(p => p.levelIndex == levelIndex);

        if (progress == null)
        {
            progress = new LevelProgress(levelIndex);
            PlayerData.levelProgressData.Add(progress);
        }

        return progress;
    }

    private void UnlockLevel(int levelIndex)
    {
        LevelProgress progress = GetLevelProgress(levelIndex);
        progress.isUnlocked = true;
    }


    public BalloonData CurrentBalloon()
    {
        var found = allBalloons.Find(b => b.id == PlayerData.currentBalloonID);

        if (found != null)
            return found;

        var fallback = allBalloons.FirstOrDefault(b => b.isUnlockedByDefault) ?? allBalloons.FirstOrDefault();
        SetCurrentBalloon(fallback.id);

        return fallback;
    }

    public void SwitchOrCreateProfile(string newName)
    {
        var existingProfile = allProfiles.Find(p => p.playerName == newName);

        if (existingProfile != null)
        {
            PlayerData = existingProfile;
        }
        else
        {
            PlayerData = new PlayerData { playerName = newName };
            allProfiles.Add(PlayerData);
        }
        SaveData();
        GameEvents.UpdatePlayerName?.Invoke();

    }


    private void OnApplicationQuit()
    {
        SaveData();
    }

}