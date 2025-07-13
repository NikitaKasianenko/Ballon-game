using UnityEngine;

public abstract class SpawnStrategy : ScriptableObject
{
    public abstract void CalculateSpawnPoint(RectTransform playerTransform, RectTransform area, out Vector3 spawnPoint, out Vector3 direction);
}