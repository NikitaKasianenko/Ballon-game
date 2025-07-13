using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HighScoreTable : MonoBehaviour
{
    [SerializeField] private Transform entryContainer, entryTemplate;
    [SerializeField] private int amount = 5;
    [SerializeField] private TextMeshProUGUI profileScore;
    [SerializeField] private Image defaultAvatar;


    private void Awake()
    {
        GameEvents.UpdatePlayerName += ShowLeaderBoard;
        GameEvents.OnPlayerAvatarChange += ShowLeaderBoard;
    }
    private void Start()
    {
        ShowLeaderBoard();
        entryTemplate.gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        GameEvents.UpdatePlayerName -= ShowLeaderBoard;
        GameEvents.OnPlayerAvatarChange -= ShowLeaderBoard;
    }

    public void ShowLeaderBoard()
    {
        ClearEntries();
        Debug.Log("[HighScoreTable] Showing leaderboard with amount: " + amount);

        RectTransform entryTemplateRectTransform = entryTemplate.GetComponent<RectTransform>();
        float high = entryTemplateRectTransform.sizeDelta.y;
        var leaderboard = DataManager.Instance.Leaderboard;

        var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        format.NumberGroupSeparator = ".";
        profileScore.text = DataManager.Instance.PlayerData.totalScore.ToString("#,0", format);
        var entriesToShow = leaderboard.Take(amount).ToList();
        if (leaderboard != null)
        {
            for (int i = 0; i < entriesToShow.Count; i++)
            {
                var entry = entriesToShow[i];
                RectTransform entryTransform = Instantiate(entryTemplate, entryContainer).GetComponent<RectTransform>();
                var entryUI = entryTransform.GetComponent<LeaderboardEntryUI>();
                entryTransform.gameObject.SetActive(true);
                entryTransform.anchoredPosition = new Vector2(0, -high * i);

                entryUI.nicknameText.text = entry.playerName;
                entryUI.scoreText.text = entry.score.ToString("#,0", format);
                Debug.Log($"[HighScoreTable] Entry {i}: Name: {entry.playerName}, Score: {entry.score}, image: {entry.avatarBase64}");
                UIManager.Instance.LoadImageFromData(entry.avatarBase64, entryUI.avatar, defaultAvatar);

            }
        }
    }
    private void ClearEntries()
    {
        foreach (Transform child in entryContainer)
        {
            if (child == entryTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
    }

}
