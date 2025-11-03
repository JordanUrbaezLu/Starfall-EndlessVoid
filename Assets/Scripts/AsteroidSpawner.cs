using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Range")]
    [SerializeField] private float minSpawnDistance = 80f;
    [SerializeField] private float maxSpawnDistance = 150f;

    [Header("Spawn Timing")]
    [SerializeField] private float spawnRate = 2f; // seconds between spawns
    private float timer;
    private float difficultyTimer;

    void Update()
    {
        if (!asteroidPrefab || !player) return;

        timer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;
            SpawnAsteroid();
        }

        // every 10s, slightly increase difficulty
        if (difficultyTimer >= 10f)
        {
            difficultyTimer = 0f;
            spawnRate = Mathf.Max(0.3f, spawnRate - 0.1f);
        }
    }

    void SpawnAsteroid()
    {
        // Random direction (anywhere around player)
        Vector3 randomDir = Random.onUnitSphere;

        // Random distance
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Calculate spawn position
        Vector3 spawnPos = player.position + randomDir * distance;

        // Spawn asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Random.rotation);
        Asteroid a = asteroid.GetComponent<Asteroid>();

        if (a != null)
            a.target = player;
    }
}
