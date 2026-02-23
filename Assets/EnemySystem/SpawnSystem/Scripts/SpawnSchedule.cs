using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Spawn Schedule")]
public class SpawnSchedule : ScriptableObject
{
    public List<SpawnEvent> spawnEvents;
}

[System.Serializable]
public class SpawnEvent
{
    public int monsterId;
    public float spawnTime;
    public int spawnPointId;
}