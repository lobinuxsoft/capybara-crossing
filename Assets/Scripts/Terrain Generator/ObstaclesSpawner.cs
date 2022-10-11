using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraCrossing
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject obs;
        [SerializeField] private int maxSize = 90;
        private ObjectPool obstaclePool;

        private void Awake()
        {
            obstaclePool = GetComponent<ObjectPool>();
            obstaclePool.InitPool(obs, maxSize);
        }

        public GameObject SpawnObstacle(Vector3 pos)
        {
            GameObject obstacle = obstaclePool.GetFromPool();
            obstacle.transform.position = pos;
            return obstacle;
        }

        public void DespawnObstacle(List<GameObject> obstacles)
        {
            foreach(GameObject obs in obstacles)
            {
                obstaclePool.ReturnToPool(obs, false);
            }
            obstacles.Clear();            
        }
    }
}
