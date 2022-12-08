using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraCrossing
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> obs = new List<GameObject>();
        [SerializeField] private int maxSize = 90;
        private ObjectPool obstaclePool;

        private void Awake()
        {
            obstaclePool = GetComponent<ObjectPool>();
            obstaclePool.InitPool(obs, maxSize);
        }

        public GameObject SpawnObstacle(Vector3 pos, bool IsRotated)
        {
            GameObject obstacle = obstaclePool.GetFromPool();
            if(obstacle.name == "Arbol(Clone)")
            {
                pos -= new Vector3(-0.5f, 0, 0);
            }
            obstacle.transform.position = pos;
            if(IsRotated)
                obstacle.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
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
