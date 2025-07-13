using UnityEngine;


[System.Serializable]
public class DartSpawnWave
{
    public string waveName;
    public GameObject dartPrefab;
    public int dartCount;
    public float spawnInterval;
    public float timeBeforeNextWave;

    public SpawnStrategy spawnStrategy;

}
