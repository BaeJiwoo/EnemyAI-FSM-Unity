using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    private Dictionary<int, GameObject> enemyDict = new();

    void Awake()
    {
        LoadAllConfigs();
    }

    void LoadAllConfigs()
    {
        EnemyConfig[] configs =
            Resources.LoadAll<EnemyConfig>("EnemyConfigs");

        foreach (var config in configs)
        {
            if (!enemyDict.ContainsKey(config.monsterId))
            {
                enemyDict.Add(config.monsterId, config.prefab);
            }
            else
            {
                Debug.LogError($"Duplicate Monster ID: {config.monsterId}");
            }
        }
    }

    public GameObject GetEnemy(int id)
    {
        if (enemyDict.TryGetValue(id, out var prefab))
            return prefab;

        Debug.LogError($"Monster ID not found: {id}");
        return null;
    }
}