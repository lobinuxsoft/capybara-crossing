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
            //powerUpPool.ReturnToPool(power, true);

            if (power.TryGetComponent(out BuffPickup pickup))
                pickup.onPickup += DespawnPowerUp;
            return power;
        }

        public static void DespawnPowerUp(GameObject powerUp)
        {
            if (powerUp.TryGetComponent(out BuffPickup pickup))
                pickup.onPickup -= DespawnPowerUp;

                powerUpPool.ReturnToPool(powerUp, false);
        }

    } 
}

