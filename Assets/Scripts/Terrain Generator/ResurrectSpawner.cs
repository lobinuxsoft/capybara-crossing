using UnityEngine;

namespace CapybaraCrossing
{
    public class ResurrectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject resurrectItem;
        private int maxSize = 4;
        private static ObjectPool resurrectItemPool;

        private void Start()
        {
            resurrectItemPool = GetComponent<ObjectPool>();
            resurrectItemPool.InitPool(resurrectItem, maxSize);
        }

        public static GameObject SpawnResurrectItem(Vector3 pos)
        {
            GameObject res = resurrectItemPool.GetFromPool();
            res.transform.position = pos;
            resurrectItemPool.ReturnToPool(res, true);
            return res;
        }

        public static void DespawnResurrectItem(GameObject resurrectItem)
        {
            resurrectItemPool.ReturnToPool(resurrectItem, false);
        }

    }
}