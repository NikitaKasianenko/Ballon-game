using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryUI : MonoBehaviour
{
    public TextMeshProUGUI nicknameText;
    public TextMeshProUGUI scoreText;
    public Image avatar;
    public Image defaulAvatar;

    private void Start()
    {
        defaulAvatar = avatar;
    }
}
