using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Button soundPlus, soundMinus;
    [SerializeField] Button musicPlus, musicMinus;
    [SerializeField] Button Save;
    private float soundVolume;
    private float musicVolume;

    SoundManager soundManager;
    [SerializeField] private Image musicFiller, soundFiller;

    private void Start()
    {
        soundManager = SoundManager.Instance;
        soundVolume = soundManager.GetSoundVolume();
        musicVolume = soundManager.GetMusicVolume();
        soundPlus.onClick.AddListener(() => AdjustVolume("Sound", 0.1f));
        soundMinus.onClick.AddListener(() => AdjustVolume("Sound", -0.1f));
        musicPlus.onClick.AddListener(() => AdjustVolume("Music", 0.1f));
        musicMinus.onClick.AddListener(() => AdjustVolume("Music", -0.1f));
        Save.onClick.AddListener(() => SaveGameSettings());
        LoadSettingsFillers();
    }

    private void AdjustVolume(string type, float delta)
    {

        if (type == "Sound")
        {
            float newVolume = soundVolume + delta;
            soundVolume = Mathf.Clamp(newVolume, 0f, 1f);
            SetSoundFiller(soundVolume);
        }
        else if (type == "Music")
        {
            float newVolume = musicVolume + delta;
            musicVolume = Mathf.Clamp(newVolume, 0f, 1f);
            SetMusicFiller(musicVolume);
        }
    }

    private void SaveGameSettings()
    {
        GameEvents.OnSoundVolumeChange?.Invoke(soundVolume);
        GameEvents.OnMusicVolumeChange?.Invoke(musicVolume);
    }

    private void LoadSettingsFillers()
    {
        if (musicFiller != null)
        {
            SetMusicFiller(musicVolume);
        }
        if (soundFiller != null)
        {
            SetSoundFiller(soundVolume);
        }
    }
    private void SetMusicFiller(float volume)
    {
        if (musicFiller != null)
        {
            musicFiller.fillAmount = volume;
        }
    }
    private void SetSoundFiller(float volume)
    {
        if (soundFiller != null)
        {
            soundFiller.fillAmount = volume;
        }
    }
}
