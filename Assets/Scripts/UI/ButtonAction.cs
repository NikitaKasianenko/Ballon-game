using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private Button MainMenu, Restart;
    [SerializeField] private TextMeshProUGUI Reward, Score;

    private void Start()
    {
        MainMenu.onClick.RemoveAllListeners();
        Restart.onClick.RemoveAllListeners();
        MainMenu.onClick.AddListener(() => GameManager.Instance.GoToMainMenu());
        Restart.onClick.AddListener(() => GameManager.Instance.RestartLevel());
    }

    public void SetReward(int reward)
    {
        Reward.text = reward.ToString();
    }

    public void SetScore(int score)
    {
        Score.text = score.ToString();
    }
}
