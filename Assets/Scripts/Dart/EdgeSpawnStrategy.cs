using UnityEngine;

[CreateAssetMenu(fileName = "Random Edge Spawn Strategy", menuName = "Game/Spawning/Random Edge")]
public class EdgeSpawnStrategy : SpawnStrategy
{
    [SerializeField] private float spawnOffset = 50f;
    public override void CalculateSpawnPoint(RectTransform playerTransform, RectTransform area, out Vector3 spawnPoint, out Vector3 direction)
    {

        if (playerTransform == null || area == null)
        {
            spawnPoint = Vector3.zero;
            direction = Vector3.zero;
            return;
        }
        float halfWidth = area.rect.width * 0.5f;
        float halfHeight = area.rect.height * 0.5f;

        int edge = Random.Range(0, 4);
        Vector2 pos = Vector2.zero;

        switch (edge)
        {
            case 0:
                pos = new Vector2(
                    Random.Range(-halfWidth, halfWidth),
                    halfHeight + spawnOffset
                );
                break;

            case 1:
                pos = new Vector2(
                    Random.Range(-halfWidth, halfWidth),
                    -halfHeight - spawnOffset
                );
                break;

            case 2:
                pos = new Vector2(
                    -halfWidth - spawnOffset,
                    Random.Range(-halfHeight, halfHeight)
                );
                break;

            case 3:
                pos = new Vector2(
                    halfWidth + spawnOffset,
                    Random.Range(-halfHeight, halfHeight)
                );
                break;
        }

        spawnPoint = pos;
        Vector2 playerPos = playerTransform.anchoredPosition;
        direction = (playerPos - pos).normalized;
    }
}