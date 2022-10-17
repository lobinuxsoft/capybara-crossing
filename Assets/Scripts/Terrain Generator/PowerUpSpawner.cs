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
        private static bool available = true;

        private void Start()
        {
            PlayerMovement.OnDeath += RemovePowerUps;
            powerUpPool = GetComponent<ObjectPool>();
            powerUpPool.InitPool(pu, maxSize);
        }

        public static GameObject SpawnPowerUp(Vector3 pos)
        {
            if (available)
            {
                GameObject power = powerUpPool.GetFromPool();
                power.transform.position = pos;
                powerUpPool.ReturnToPool(power, true);
                return power;
            }
            else
            {
                return null;
            }
        }

        public static void DespawnPowerUp(GameObject powerUp)
        {
            powerUpPool.ReturnToPool(powerUp, false);
        }

        private void RemovePowerUps()
        {
            available = false;
            powerUpPool.ResetPool(false);
        }
    } 
}

