using UnityEngine;

namespace CapybaraCrossing
{
    public class TileGenerator : MonoBehaviour
    {
        [SerializeField] TileData tileData;
        [SerializeField] private int tileWidth = 50;

        [SerializeField] GameObject power;
        [SerializeField] GameObject obst;

        public TileData TileData
        {
            get => tileData;
            set
            {
                tileData = value;
                UpdateTileData();
            }
        }

        public int TileWidth
        {
            get => tileWidth;
            set => tileWidth = value;
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

        public void UpdateTileData()
        {
            Random.InitState(Mathf.RoundToInt(Time.time));
            int rand = Random.Range(0,tileData.TileObjects.Length);
            for (int i = 0; i < TileWidth; i++)
            { 
                if(transform.GetChild(i).TryGetComponent(out MeshFilter filter))
                {
                    filter.mesh = tileData.TileObjects[rand].mesh;
                }
                if (transform.GetChild(i).TryGetComponent(out MeshRenderer renderer))
                {
                    renderer.material = tileData.TileObjects[rand].material;
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

    }
}