using System.Collections.Generic;
using UnityEngine;

namespace CapybaraCrossing
{
    public class TileGenerator : MonoBehaviour
    {
        [SerializeField] TileData tileData;
        [SerializeField] private int tileWidth = 50;

        private TypeOfTile type;

        private List<GameObject> obstacles = new List<GameObject>();

        private GameObject powerUp;

        private List<GameObject> deepWaters = new List<GameObject>();

        private static int lastPowerUpPositionZ = 0;


        public List<GameObject> Obstacles
        {
            get => obstacles;
        }

        public TileData TileData
        {
            get => tileData;
            set
            {
                tileData = value;
            }
        }

        public int TileWidth
        {
            get => tileWidth;
            set => tileWidth = value;
        }

        private void Awake()
        {
            TerrainGenerator.OnTileChange += RemoveObstacles;   
        }

        private void OnDestroy()
        {
            TerrainGenerator.OnTileChange -= RemoveObstacles;
        }

        public void GenerateTile()
        {
            for (int i = 0; i < TileWidth; i++)
            {
                GameObject go = new GameObject($"{i}");
                go.transform.SetParent(transform);
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                go.AddComponent<BoxCollider>();
                go.transform.localPosition = transform.right * i;
            }
            Recenter();
        }

        public void UpdateFirstTileData()
        {
            type = (TypeOfTile)1;
            int maxObstaclePerLine = 3;
            int currentAmountOfObstacles = 0;
            for (int i = 0; i < TileWidth; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out MeshFilter filter))
                {
                    filter.mesh = tileData.TileObjects[(int)type].mesh;
                }
                if (transform.GetChild(i).TryGetComponent(out MeshRenderer renderer))
                {
                    renderer.material = tileData.TileObjects[(int)type].material;
                }
                if (currentAmountOfObstacles < maxObstaclePerLine && transform.position.z != 0)
                {
                    if (Random.Range(0, 5) == 1)
                    {
                        obstacles.Add(ObstacleSpawnerManager.Instance.SpawnObstacle("RockPool", new Vector3(transform.GetChild(i).position.x, 0.65f, transform.position.z)));
                        currentAmountOfObstacles++;
                    }
                }
            }
        }

        public void UpdateTileData()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            type = (TypeOfTile)Random.Range(0,tileData.TileObjects.Length);
            int maxRockPerLine = 3;
            int currentAmountOfRock = 0;
            int maxDeepWaterPerLine = 12;
            int currentAmountOfDeepWaters = 0;
            for (int i = 0; i < TileWidth; i++)
            { 
                if(transform.GetChild(i).TryGetComponent(out MeshFilter filter))
                {
                    filter.mesh = tileData.TileObjects[(int)type].mesh;
                }
                if (transform.GetChild(i).TryGetComponent(out MeshRenderer renderer))
                {
                    renderer.material = tileData.TileObjects[(int)type].material;
                }
                switch (type)
                {
                    case TypeOfTile.GRASS:
                        if(transform.position.z - lastPowerUpPositionZ >= 10 && powerUp == null)
                        {
                            if(Random.Range(0, 5) == 1)
                            {
                                lastPowerUpPositionZ = (int)transform.position.z;
                                powerUp = PowerUpSpawner.SpawnPowerUp(new Vector3(transform.GetChild(i).position.x, 1, transform.position.z));
                            }
                        }
                        else if(currentAmountOfRock < maxRockPerLine)
                        {
                            if(Random.Range(0, 5) == 1)
                            {
                                obstacles.Add(ObstacleSpawnerManager.Instance.SpawnObstacle("RockPool", new Vector3(transform.GetChild(i).position.x, 0.65f, transform.position.z)));
                                currentAmountOfRock++;
                            }
                        }
                        break;
                    case TypeOfTile.ROAD:
                        if(i == 0)
                        {
                            obstacles.Add(ObstacleSpawnerManager.Instance.SpawnObstacle("CarPool", new Vector3(0,0, transform.position.z)));
                        }
                        break;
                    case TypeOfTile.WATER:
                        if (currentAmountOfDeepWaters < maxDeepWaterPerLine)
                        {
                            if (Random.Range(0, 2) == 1)
                            {
                                GameObject deepWater = transform.GetChild(i).gameObject;
                                deepWater.transform.GetComponent<Collider>().enabled = false;
                                deepWaters.Add(deepWater);
                                currentAmountOfDeepWaters++;
                            }
                            else
                            {
                                obstacles.Add(ObstacleSpawnerManager.Instance.SpawnObstacle("CamalotePool", new Vector3(transform.GetChild(i).position.x, -0.5f, transform.position.z)));
                            }
                        }
                        else
                        {
                            obstacles.Add(ObstacleSpawnerManager.Instance.SpawnObstacle("CamalotePool", new Vector3(transform.GetChild(i).position.x, -0.5f, transform.position.z)));
                        }
                        break;
                    case TypeOfTile.TRAIN:
                        break;
                }
            }
        }

        void Recenter()
        {
            if (transform.childCount > 0)
            {
                float distance = Vector3.Distance(transform.GetChild(0).position, transform.GetChild(transform.childCount - 1).position);

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).localPosition += (-transform.right * distance / 2 + Vector3.right / 2);
                }
            }
        }

        private void RemoveObstacles(GameObject tile)
        {
            if(tile == transform.gameObject)
            {
                switch (type)
                {
                    case TypeOfTile.GRASS:
                        ObstacleSpawnerManager.Instance.DespawnObstacle("RockPool", obstacles);
                        if (powerUp)
                        {
                            PowerUpSpawner.DespawnPowerUp(powerUp);
                        }
                        break;
                    case TypeOfTile.ROAD:
                        ObstacleSpawnerManager.Instance.DespawnObstacle("CarPool", obstacles);
                        break;
                    case TypeOfTile.WATER:
                        ObstacleSpawnerManager.Instance.DespawnObstacle("CamalotePool", obstacles);
                        foreach (GameObject deepWater in deepWaters)
                        {
                            deepWater.transform.GetComponent<Collider>().enabled = true;
                        }
                        break;
                    case TypeOfTile.TRAIN:
                        break;
                }
            } 
        }

    }
}