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

        public static void SpawnObstacle(Vector3 pos)
        {
            GameObject obs = obstaclePool.GetFromPool();
            obs.transform.position = pos;
            obs.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            obstaclePool.ReturnToPool(obs, true);
        }
    }
}
