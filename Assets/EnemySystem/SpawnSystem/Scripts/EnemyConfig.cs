using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public int monsterId;
    public GameObject prefab;
}