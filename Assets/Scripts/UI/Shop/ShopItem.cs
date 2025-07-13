using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ShopItem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image balloonImage;
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    private BalloonData data;
    [SerializeField] private string BalloonDataID;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        data = DataManager.Instance.GetBallonById(BalloonDataID);
        GameEvents.OnPlayerDataUpdate += UpdateButtonState;
        GameEvents.UpdatePlayerName += UpdateButtonState;
        balloonImage.sprite = data.balloonSprite;
        UpdateButtonState();
        actionButton.onClick.AddListener(() => GameEvents.OnShopItemClicked.Invoke(data));
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerDataUpdate -= UpdateButtonState;

        actionButton.onClick.RemoveAllListeners();
    }

    public void UpdateButtonState()
    {
        var profile = DataManager.Instance.PlayerData;
        bool unlocked = DataManager.Instance.IsBalloonUnlocked(data.id);
        bool selected = profile.currentBalloonID == data.id;

        if (unlocked)
        {
            buttonText.text = selected ? "Selected" : "Owned";
            actionButton.interactable = !selected;
        }
        else
        {
            buttonText.text = data.price.ToString();
            actionButton.interactable = profile.coins >= data.price;
        }
    }
}