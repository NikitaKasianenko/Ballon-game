using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Image progressBar;
    private float target;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loaderCanvas);
        }
        else
        {
            Destroy(loaderCanvas);
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 2 * Time.deltaTime);
        }

    }

    public async void LoadScene(string sceneName)
    {

        Debug.Log($"Loading scene: {sceneName}");
        target = 0f;
        progressBar.fillAmount = 0f;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loaderCanvas.SetActive(true);

        do
        {
            await System.Threading.Tasks.Task.Delay(100); // just for visual effect
            target = scene.progress;
        } while (scene.progress < 0.9f);

        await System.Threading.Tasks.Task.Delay(1000); // just for visual effect

        scene.allowSceneActivation = true;
        loaderCanvas.SetActive(false);
    }
}
