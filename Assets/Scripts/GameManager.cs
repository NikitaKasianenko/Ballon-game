using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private GameObject WinScreen, LoseScreen;
    public static GameManager Instance { get; private set; }
    private ScoreSystem scoreSystem = new ScoreSystem();

    private SoundData soundData;
    public LevelData CurrentLevelData { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += HandleInit;
            GameEvents.OnPlayerWin += onWin;
            GameEvents.OnPlayerLose += onLose;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        soundData = DataManager.Instance.soundData;
        SoundManager.Instance.PlayLoop(soundData.inMainMenu);
        GameEvents.OnPlayerWin -= onWin;
        GameEvents.OnPlayerLose -= onLose;
        GameEvents.OnPlayerWin += onWin;
        GameEvents.OnPlayerLose += onLose;
        //   for build perpose purpose
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleInit;
    }

    private void onWin(int curhealth, int maxHealth)
    {
        HandleLevel(curhealth, maxHealth, true);
    }
    private void onLose(int curhealth, int maxHealth)
    {
        HandleLevel(curhealth, maxHealth, false);
    }
    private void HandleInit(Scene scene, LoadSceneMode mode)
    {

        if (scene.name != Tags.GameScene)
        {
            return;
        }
        SoundManager.Instance.PlayLoop(soundData.inGame);

        if (WinScreen == null || LoseScreen == null)
        {
            LoseScreen = GameObject.Find(Tags.LoseScreen);
            WinScreen = GameObject.Find(Tags.WinScreen);
        }

        LoseScreen.SetActive(false);
        WinScreen.SetActive(false);

    }
    private void HandleLevel(int currentHeath, int maxHeath, bool completed)
    {
        SoundManager.Instance.StopLoop();
        ButtonAction buttonAction;
        if (completed)
        {

            SoundManager.Instance.PlaySFX(soundData.gameWin);
            WinScreen.SetActive(true);
            buttonAction = WinScreen.GetComponent<ButtonAction>();
        }
        else
        {
            SoundManager.Instance.PlaySFX(soundData.gameOver);
            LoseScreen.SetActive(true);
            buttonAction = LoseScreen.GetComponent<ButtonAction>();
        }
        Debug.Log($"Level '{CurrentLevelData.name}' completed!");

        int score = scoreSystem.CalculateScore(currentHeath, maxHeath);
        int reward = scoreSystem.CalculateReward(currentHeath, maxHeath);
        int stars = completed ? scoreSystem.GetStarRating(currentHeath, maxHeath) : 0;

        buttonAction.SetReward(reward);
        buttonAction.SetScore(score);

        DataManager.Instance.UpdateLevelProgress(CurrentLevelData.levelIndex, score, stars);
        DataManager.Instance.AddCoins(reward);
        DataManager.Instance.SaveData();
    }


    public void StartLevel(LevelData levelToLoad)
    {


        if (levelToLoad == null)
        {
            return;
        }

        if (DataManager.Instance.GetLevelProgress(levelToLoad.levelIndex).isUnlocked)
        {
            CurrentLevelData = levelToLoad;
            Debug.Log($"starting level '{CurrentLevelData.name}' (Index: {CurrentLevelData.levelIndex})...");
            LevelManager.Instance.LoadScene(Tags.GameScene);

        }
        else
        {
            Debug.Log($"trying to start a locked level: {levelToLoad.name}");
        }
    }

    public void GoToMainMenu()
    {
        Debug.Log("Returning to main menu...");
        SoundManager.Instance.PlayLoop(soundData.inMainMenu);
        LevelManager.Instance.LoadScene(Tags.MenuScene);
    }

    public void RestartLevel()
    {
        LevelManager.Instance.LoadScene(Tags.GameScene);
    }

}