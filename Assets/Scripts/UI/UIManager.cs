using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] playerAvatars;
    [SerializeField] private TextMeshProUGUI[] playerNicknameTexts;
    [SerializeField] private Button exitButton;
    private Image defaultAvatar;

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        defaultAvatar = playerAvatars[0];
        LoadAvatarFromPlayerData();
        LoadPlayerNameFromData();
        GameEvents.UpdatePlayerName += LoadPlayerNameFromData;
        GameEvents.OnPlayerAvatarChange += LoadAvatarFromPlayerData;
        exitButton.onClick.AddListener(() => Application.Quit());
    }



    private void OnDestroy()
    {
        GameEvents.UpdatePlayerName -= LoadPlayerNameFromData;
        GameEvents.OnPlayerAvatarChange -= LoadAvatarFromPlayerData;
    }

    public void LoadAvatarFromPlayerData()
    {
        string base64 = DataManager.Instance.PlayerData.avatarBase64;

        foreach (var avatar in playerAvatars)
        {
            LoadImageFromData(base64, avatar, defaultAvatar);
        }
    }

    public void LoadPlayerNameFromData()
    {
        LoadAvatarFromPlayerData();
        Debug.Log("[UIManager] Loading player name from data.");
        string playerName = DataManager.Instance.PlayerData.playerName;
        foreach (var nameText in this.playerNicknameTexts)
        {
            nameText.text = playerName;
            Debug.Log($"[UIManager] Player name set to: {playerName} in UI.");
        }


    }

    public void LoadImageFromData(string base64, Image image, Image defaulImage)
    {
        if (!string.IsNullOrEmpty(base64))
        {
            byte[] bytes = System.Convert.FromBase64String(base64);
            Texture2D tex = new Texture2D(2, 2);
            if (tex.LoadImage(bytes))
            {
                image.sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    Vector2.one * 0.5f
                );
            }
            return;

        }
        image.sprite = defaulImage.sprite;
    }
}
