using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSource;

    private int volumeSound;
    private int volumeMusic;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetMusicVolume(DataManager.Instance.gameSettings.musicVolume);
        SetSoundVolume(DataManager.Instance.gameSettings.soundVolume);
        GameEvents.OnMusicVolumeChange += SetMusicVolume;
        GameEvents.OnSoundVolumeChange += SetSoundVolume;
    }

    private void OnDestroy()
    {
        GameEvents.OnMusicVolumeChange -= SetMusicVolume;
        GameEvents.OnSoundVolumeChange -= SetSoundVolume;
    }

    private void SetMusicVolume(float volume)
    {
        if (loopSource != null)
        {
            if (volume < 0 || volume > 1)
            {
                volume = Mathf.Clamp01(volume);
            }
            loopSource.volume = volume;
            DataManager.Instance.gameSettings.musicVolume = volume;
        }
    }

    private void SetSoundVolume(float volume)
    {
        if (sfxSource != null)
        {
            if (volume < 0 || volume > 1)
            {
                volume = Mathf.Clamp01(volume);
            }
            sfxSource.volume = volume;
            DataManager.Instance.gameSettings.soundVolume = volume;

        }
    }

    public float GetMusicVolume()
    {
        return loopSource.volume;
    }

    public float GetSoundVolume()
    {
        return sfxSource.volume;
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayLoop(AudioClip clip)
    {
        if (clip == null) return;

        if (loopSource.isPlaying || loopSource.clip != clip)
        {
            loopSource.Stop();
            loopSource.clip = clip;
            loopSource.loop = true;
            loopSource.Play();
        }
        else if (!loopSource.isPlaying)
        {
            loopSource.Play();
        }
    }

    public void StopLoop()
    {
        loopSource.Stop();
    }

}