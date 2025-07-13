using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    [Header("Shop Setup")]
    [SerializeField] private GameObject confirmWindow;
    [SerializeField] private Image confirmBalloonImage;
    [SerializeField] private Button buyButton;
    private TextMeshProUGUI buyButtonText;

    [SerializeField] private Button cancelButton;

    private BalloonData currentSelection;

    private void Start()
    {
        buyButtonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
        GameEvents.OnShopItemClicked += OnShopItemClicked;
        confirmWindow.SetActive(false);
        buyButton.onClick.AddListener(OnConfirmBuy);
        cancelButton.onClick.AddListener(() => confirmWindow.SetActive(false));
    }

    private void OnShopItemClicked(BalloonData data)
    {
        currentSelection = data;
        confirmWindow.SetActive(true);

        buyButtonText.text = DataManager.Instance.IsBalloonUnlocked(data.id) ? "Select" : "Buy";

        confirmBalloonImage.sprite = data.balloonSprite;
    }

    private void OnConfirmBuy()
    {
        var profile = DataManager.Instance.PlayerData;
        bool unlocked = DataManager.Instance.IsBalloonUnlocked(currentSelection.id);

        if (unlocked)
        {
            DataManager.Instance.SetCurrentBalloon(currentSelection.id);
        }
        else
        {
            if (DataManager.Instance.SpendCoins(currentSelection.price))
            {
                DataManager.Instance.UnlockBalloon(currentSelection.id);
                DataManager.Instance.SetCurrentBalloon(currentSelection.id);
                Debug.Log($"Balloon '{currentSelection.balloonName}' purchased successfully!");
            }
            else
            {
                buyButton.interactable = false;
                return;
            }
        }
        GameEvents.OnPlayerDataUpdate?.Invoke();
        confirmWindow.SetActive(false);
    }
}
