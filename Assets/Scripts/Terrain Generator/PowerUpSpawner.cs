using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraCrossing
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject pu;
        private int maxSize = 5;
        private static ObjectPool powerUpPool;

        private void Start()
        {
            powerUpPool = GetComponent<ObjectPool>();
            powerUpPool.InitPool(pu, maxSize);
        }

        public static GameObject SpawnPowerUp(Vector3 pos)
        {
            GameObject power = powerUpPool.GetFromPool();
            power.transform.position = pos;
            powerUpPool.ReturnToPool(power, true);
            return power;
        }

        public static void DespawnPowerUp(GameObject powerUp)
        {
            powerUpPool.ReturnToPool(powerUp, false);
        }

    } 
}

