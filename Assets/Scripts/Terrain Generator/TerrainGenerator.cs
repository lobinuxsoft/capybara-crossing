using System.Collections;
using UnityEngine;


namespace CapybaraCrossing
{
    [RequireComponent(typeof(ObjectPool))]
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private int maxTerrainCount;
        [SerializeField] private int tileWidth = 20;
        [SerializeField] private TileGenerator terrainPref;
        [SerializeField] TileData tileData;

        private ObjectPool pool;
        private Vector3 currentPosition = Vector3.zero;

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
                    tileGenerator.TileData = tileData;
                    tileGenerator.GenerateTile();
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
                    tile.transform.position += new Vector3(0, 0, maxTerrainCount);
                    if(currentPosition.z % 10 == 0)
                    {
                        int posX = Random.Range((int)tile.transform.GetChild(0).position.x, (int)tile.transform.GetChild(tile.transform.childCount - 1).position.x);
                        PowerUpSpawner.SpawnPowerUp(new Vector3(posX, 1, currentPosition.z));
                    }
                    currentPosition.z++;
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
            tile.transform.position += new Vector3(0, 0, maxTerrainCount);
            if (currentPosition.z % 10 == 0)
            {
                int posX = Random.Range((int)tile.transform.GetChild(0).position.x, (int)tile.transform.GetChild(tile.transform.childCount - 1).position.x);
                PowerUpSpawner.SpawnPowerUp(new Vector3(posX, 1, currentPosition.z));
            }
            else
            {
                if(currentPosition.z != 0)
                {
                    for(int i = 0; i < 3; i++)
                    {
                        int posX = Random.Range((int)tile.transform.GetChild(0).position.x, (int)tile.transform.GetChild(tile.transform.childCount - 1).position.x);
                        ObstaclesSpawner.SpawnObstacle(new Vector3(posX, 0.6f, currentPosition.z));
                    }
                    
                }
            }
            
            currentPosition.z++;
            pool.ReturnToPool(tile, true);
        }
    }
}