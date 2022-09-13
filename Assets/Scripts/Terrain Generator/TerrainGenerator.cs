using UnityEngine;

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
        PlayerMovement.OnJump += CheckSpawnTerrain;

        pool = GetComponent<ObjectPool>();
        pool.InitPool(terrainPref.gameObject, maxTerrainCount);

        for (int i = 0; i < pool.Size; i++)
        {
            GameObject temp = pool.GetFromPool();
            temp.transform.localPosition = currentPosition;

            if(temp.TryGetComponent(out TileGenerator tileGenerator))
            {
                tileGenerator.TileWidth = tileWidth;
                tileGenerator.TileData = tileData;
                StartCoroutine(tileGenerator.GenerateTile());
            }

            pool.ReturnToPool(temp, true);
            currentPosition.z++;
        }
    }

    private void SpawnTerrain()
    {
        GameObject temp = pool.GetFromPool();
        temp.transform.localPosition = currentPosition;

        if (temp.TryGetComponent(out TileGenerator tileGenerator))
        {
            tileGenerator.TileWidth = tileWidth;
            tileGenerator.TileData = tileData;
            StartCoroutine(tileGenerator.GenerateTile());
        }

        pool.ReturnToPool(temp, true);
        currentPosition.z++;
    }

    void CheckSpawnTerrain(int playerZ)
    {
        if(transform.GetChild(transform.childCount-1).position.z - playerZ < maxTerrainCount / 2)
        {
            SpawnTerrain();
        }
    }
}