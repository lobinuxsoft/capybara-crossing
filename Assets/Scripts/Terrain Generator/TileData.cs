using UnityEngine;

[CreateAssetMenu(menuName = "Terrain Generator/ Tile Data")]
public class TileData : ScriptableObject
{
    [SerializeField] GameObject[] tileObjects;

    public GameObject GetRandomTileObject(int rnd)
    {
        Random.InitState(rnd);

        return tileObjects[Random.Range(0, tileObjects.Length)];
    }
}
