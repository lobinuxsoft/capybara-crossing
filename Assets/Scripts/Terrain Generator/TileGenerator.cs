using System.Collections;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] TileData tileData;
    [SerializeField] private int tileWidth = 50;

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

    public IEnumerator GenerateTile()
    {
        yield return StartCoroutine(DestroyTile());

        for (int i = 0; i < TileWidth; i++)
        {
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
                transform.GetChild(i).localPosition += (-transform.right * distance / 2);
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
