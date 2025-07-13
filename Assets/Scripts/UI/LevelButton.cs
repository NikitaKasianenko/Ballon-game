using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject Star1, Star2, Star3;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned on this button!", this.gameObject);
            button.interactable = false;
            return;
        }

        button.onClick.AddListener(() => GameManager.Instance.StartLevel(levelData));
        SetupButtonVisuals();
        GameEvents.UpdatePlayerName += SetupButtonVisuals;
    }

    private void OnDestroy()
    {
        GameEvents.UpdatePlayerName -= SetupButtonVisuals;
    }


    private void SetupButtonVisuals()
    {
        LevelProgress progress = DataManager.Instance.GetLevelProgress(levelData.levelIndex);


        if (progress.isUnlocked)
        {
            button.interactable = true;
            if (lockIcon != null) lockIcon.SetActive(false);
        }
        else
        {
            button.interactable = false;
            if (lockIcon != null) lockIcon.SetActive(true);
        }

        SetupStars(progress.stars);
    }

    private void SetupStars(int stars)
    {
        switch (stars)
        {
            case 0:
                Star1.SetActive(false);
                Star2.SetActive(false);
                Star3.SetActive(false);
                break;
            case 1:
                Star1.SetActive(true);
                Star2.SetActive(false);
                Star3.SetActive(false);
                break;
            case 2:
                Star1.SetActive(true);
                Star2.SetActive(true);
                Star3.SetActive(false);
                break;
            case 3:
                Star1.SetActive(true);
                Star2.SetActive(true);
                Star3.SetActive(true);
                break;
        }

    }

}
