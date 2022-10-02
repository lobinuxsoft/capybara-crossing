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
            set => tileData = value;
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
                GameObject go = Instantiate(tileData.GetRandomTileObject(Mathf.RoundToInt(Time.time)), transform);
                go.transform.localPosition = transform.right * i;
            }
            Recenter();
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