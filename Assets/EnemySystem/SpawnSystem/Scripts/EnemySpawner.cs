using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyRegistry registry;
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
        GameObject prefab = registry.GetEnemy(data.monsterId);
        if (prefab == null) return;

        Instantiate(prefab, data.spawnPosition, Quaternion.identity);
    }
}