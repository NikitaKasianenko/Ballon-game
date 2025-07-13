using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartSpawner : MonoBehaviour
{
    public LevelData levelData;
    public RectTransform playerTransform;
    [SerializeField] private RectTransform area;
    private List<Dart> darts = new List<Dart>();
    private Coroutine waveCoroutine;
    SoundData soundData;
    SoundManager soundManager;

    private int currentWaveIndex = 0;
    private int dartsLeft = 0;
    [Header("Win check time")]
    [SerializeField] private float timeToWin;

    void Start()
    {
        soundData = DataManager.Instance.soundData;
        soundManager = SoundManager.Instance;
        GameEvents.OnWavesEnd += HandleStop;
        if (levelData.waves.Length > 0)
        {
            waveCoroutine = StartCoroutine(WaveRoutine());
        }
        levelData = GameManager.Instance.CurrentLevelData;
    }

    private void HandleStop()
    {
        if (this != null && waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
        }
        ClearDarts();
    }

    private void OnDestroy()
    {
        GameEvents.OnWavesEnd -= HandleStop;
    }

    private IEnumerator WaveRoutine()
    {

        while (currentWaveIndex < levelData.waves.Length)
        {
            var wave = levelData.waves[currentWaveIndex];

            yield return new WaitForSeconds(wave.timeBeforeNextWave);
            ClearDarts();
            dartsLeft = wave.dartCount;

            while (dartsLeft > 0)
            {
                SpawnOne(wave);
                dartsLeft--;

                yield return new WaitForSeconds(wave.spawnInterval);
            }

            currentWaveIndex++;
        }

        yield return new WaitForSeconds(timeToWin);
        GameEvents.OnWavesEnd?.Invoke();

    }

    private void ClearDarts()
    {
        foreach (var dart in darts)
        {
            if (dart != null)
            {
                Destroy(dart.gameObject);
            }
        }
        darts.Clear();
    }

    private void SpawnOne(DartSpawnWave wave)
    {
        Vector3 spawnPoint, direction;
        wave.spawnStrategy.CalculateSpawnPoint(playerTransform, area, out spawnPoint, out direction);

        Vector2 spawnPos = spawnPoint;
        Vector2 dartDirection = direction.normalized;

        var dart = Instantiate(wave.dartPrefab, area);
        var rt = dart.GetComponent<RectTransform>();
        rt.anchoredPosition = spawnPos;

        var dartUI = dart.GetComponent<Dart>();
        dartUI.Initialize(direction);
        darts.Add(dartUI);
        soundManager.PlaySFX(soundData.enemeSpawn);
    }
}
