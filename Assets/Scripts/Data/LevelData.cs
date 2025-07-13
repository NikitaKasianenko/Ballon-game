using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public DartSpawnWave[] waves;
}
