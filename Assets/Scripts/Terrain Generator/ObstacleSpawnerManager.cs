using CapybaraCrossing;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnerManager : MonoBehaviour
{
    static ObstacleSpawnerManager instance;

    public static ObstacleSpawnerManager Instance
    {
        get => instance;
    }

    Dictionary<string, ObstaclesSpawner> spawners = new Dictionary<string, ObstaclesSpawner>();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            var temp = transform.GetChild(i).GetComponent<ObstaclesSpawner>();
            spawners.Add(temp.name, temp);
        }
    }

    public GameObject SpawnObstacle(string spawnerName, Vector3 posToSpawn)
    {
        GameObject obstacle = spawners[spawnerName].SpawnObstacle(posToSpawn);
        return obstacle;
    }

    public void DespawnObstacle(string spawnerName, List<GameObject> obstacles)
    {
        spawners[spawnerName].DespawnObstacle(obstacles);
    }
}