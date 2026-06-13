using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject healthItemPrefab;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public float spawnInterval = 15f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnItem), spawnInterval, spawnInterval);
    }

    void SpawnItem()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            0f // Z fixé à 0 pour une scène 2D XY
        );

        Instantiate(healthItemPrefab, spawnPos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3(
            (spawnAreaMin.x + spawnAreaMax.x) / 2,
            (spawnAreaMin.y + spawnAreaMax.y) / 2,
            0f
        );
        Vector3 size = new Vector3(
            Mathf.Abs(spawnAreaMax.x - spawnAreaMin.x),
            Mathf.Abs(spawnAreaMax.y - spawnAreaMin.y),
            0.1f // mince zone pour qu'elle soit visible dans la scène
        );

        Gizmos.DrawWireCube(center, size);
    }
}
