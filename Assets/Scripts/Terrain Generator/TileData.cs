using UnityEngine;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Terrain Generator/ Tile Data")]
    public class TileData : ScriptableObject
    {
        [SerializeField] TileStruct[] tileObjects;

        public TileStruct[] TileObjects
        {
            get => tileObjects;
        }
    }

    [System.Serializable]
    public struct TileStruct
    {
        public Mesh mesh;
        public Material material;
    }
}