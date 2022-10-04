using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraCrossing
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject obs;
        private int maxSize = 90;
        private static ObjectPool obstaclePool;

        private void Start()
        {
            obstaclePool = GetComponent<ObjectPool>();
            obstaclePool.InitPool(obs, maxSize);
        }

        public ObjectPool SpawnObstacle(Vector3 pos, out GameObject obstacle)
        {
            obstacle = obstaclePool.GetFromPool();
            obstacle.transform.position = pos;
            obstacle.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            //obstaclePool.ReturnToPool(obstacle, true);
            return obstaclePool;
        }
    }
}
