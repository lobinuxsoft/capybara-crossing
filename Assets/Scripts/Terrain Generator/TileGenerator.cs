using System.Collections;
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

        public IEnumerator GenerateTile(bool powerUp, bool obstacle)
        {
            yield return StartCoroutine(DestroyTile());
            int powerSpawnedPosition = Random.Range(0,TileWidth);
            int obstacleCount = 0;
            int rand = 0;
            for (int i = 0; i < TileWidth; i++)
            {
                Random.InitState(System.DateTime.Now.Millisecond * i);
                rand = Random.Range(0, 100);
                bool obstacleSpawn = rand < 25;
                if (powerUp && i == powerSpawnedPosition)
                {
                    GameObject pu = Instantiate(power, transform);
                    pu.transform.localPosition = transform.right * i;
                    pu.transform.localPosition += Vector3.up;
                }
                int a = Mathf.RoundToInt(Time.time);
                if (obstacle && obstacleSpawn && i != powerSpawnedPosition && obstacleCount < 3)
                {
                    obstacleCount++;
                    GameObject obs = Instantiate(obst, transform);
                    obs.transform.localPosition = transform.right * i;
                    obs.transform.localPosition += new Vector3(0,0.6f,0);
                }
                GameObject go = Instantiate(tileData.GetRandomTileObject(Mathf.RoundToInt(Time.time)), transform);
                go.transform.localPosition = transform.right * i;
            }

            yield return new WaitForEndOfFrame();

            Recenter();
        }

        void Recenter()
        {
            if(transform.childCount > 0)
            {
                float distance = Vector3.Distance(transform.GetChild(0).position, transform.GetChild(transform.childCount - 1).position);

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).localPosition += (-transform.right * distance / 2 + Vector3.right/2);
                }
            }
        }

        IEnumerator DestroyTile()
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}