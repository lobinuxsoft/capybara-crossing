using System.Collections;
using UnityEngine;
using System;

namespace CapybaraCrossing
{
    [RequireComponent(typeof(ObjectPool))]
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private int maxTerrainCount;
        [SerializeField] private int tileWidth = 34;
        [SerializeField] private TileGenerator terrainPref;
        [SerializeField] TileData tileData;

        private ObjectPool pool;
        private Vector3 currentPosition = Vector3.zero;

        public static Action<GameObject> OnTileChange;

        private void Start()
        {
            InvokeRepeating("SpawnOneTile", 10f, 1f);

            PlayerMovement.OnJump += CheckSpawnTerrain;

            pool = GetComponent<ObjectPool>();
            pool.InitPool(terrainPref.gameObject, maxTerrainCount);

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject temp = pool.GetFromPool();
                temp.transform.localPosition = currentPosition;

                if (temp.TryGetComponent(out TileGenerator tileGenerator))
                {
                    tileGenerator.TileWidth = tileWidth;
                    tileGenerator.GenerateTile();
                    tileGenerator.TileData = tileData;
                    tileGenerator.UpdateFirstTileData();
                }
                pool.ReturnToPool(temp, true);
                currentPosition.z++;
            }
        }

        private void OnDestroy()
        {
            PlayerMovement.OnJump -= CheckSpawnTerrain;
        }

        private IEnumerator SpawnTerrain(int position)
        {
            int tilePosition = (int)currentPosition.z - position;
            int middleOfMap = maxTerrainCount / 2;
            if (tilePosition < middleOfMap)
            {
                for(int i = 0; i < middleOfMap - tilePosition; i++)
                {
                    GameObject tile = pool.GetFromPool();
                    OnTileChange(tile);
                    tile.transform.position += new Vector3(0, 0, maxTerrainCount);
                    currentPosition.z++;
                    tile.GetComponent<TileGenerator>().UpdateTileData();
                    pool.ReturnToPool(tile, true);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        void CheckSpawnTerrain(int playerZ)
        {
            StartCoroutine(SpawnTerrain(playerZ));
        }

        private void SpawnOneTile()
        {
            GameObject tile = pool.GetFromPool();
            OnTileChange(tile);
            tile.transform.position += new Vector3(0, 0, maxTerrainCount);
            currentPosition.z++;
            tile.GetComponent<TileGenerator>().UpdateTileData();
            pool.ReturnToPool(tile, true);
        }
    }
}