using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyRegistry enemyRegistry;
    public SpawnPointRegistry spawnPointRegistry;
    public SpawnSchedule schedule;

    private float timer = 0f;
    private int currentIndex = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (currentIndex >= schedule.spawnEvents.Count)
            return;

        SpawnEvent spawnEvent = schedule.spawnEvents[currentIndex];

        if (timer >= spawnEvent.spawnTime)
        {
            Spawn(spawnEvent);
            currentIndex++;
        }
    }

    void Spawn(SpawnEvent data)
    {
        GameObject prefab = enemyRegistry.GetEnemy(data.monsterId);
        if (prefab == null) return;

        Transform spawnPoint =
            spawnPointRegistry.GetSpawnPoint(data.spawnPointId);

        if (spawnPoint == null) return;

        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}