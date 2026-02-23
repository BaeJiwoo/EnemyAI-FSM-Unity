using System.Collections.Generic;
using UnityEngine;

public class SpawnPointRegistry : MonoBehaviour
{
    private Dictionary<int, Transform> spawnPointDict = new();

    void Awake()
    {
        RegisterAllSpawnPoints();
    }

    void RegisterAllSpawnPoints()
    {
        SpawnPoint[] points = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        foreach (var point in points)
        {
            if (!spawnPointDict.ContainsKey(point.spawnPointId))
                spawnPointDict.Add(point.spawnPointId, point.transform);
            else
                Debug.LogError($"Duplicate SpawnPoint ID: {point.spawnPointId}");
        }

        Debug.Log(spawnPointDict.Count);
    }

    public Transform GetSpawnPoint(int id)
    {
        if (spawnPointDict.TryGetValue(id, out var transform))
            return transform;

        Debug.LogError($"SpawnPoint ID not found: {id}");
        return null;
    }
}