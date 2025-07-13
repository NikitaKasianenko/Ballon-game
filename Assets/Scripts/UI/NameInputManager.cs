using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    private string playerName;
    [SerializeField] private Button submitButton;
    [SerializeField] private bool changeProfile = false;

    void Start()
    {
        nameInputField.onValueChanged.AddListener(OnNameChanged);
        submitButton.onClick.AddListener(OnSubmit);

    }
    public void OnNameChanged(string newName)
    {
        playerName = newName;
        Debug.Log("Name changed: " + playerName);

    }

    public void OnSubmit()
    {
        if (changeProfile)
        {
            DataManager.Instance.SwitchOrCreateProfile(playerName);
            GameEvents.OnPlayerAvatarChange?.Invoke();
        }
        else
        {
            DataManager.Instance.ChangeCurrentProfileName(playerName);
        }

        Debug.Log("Name submitted: " + playerName);
    }
}
