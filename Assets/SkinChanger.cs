using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    [SerializeField] private List<Material> skins;
    private SkinnedMeshRenderer mr;

    private void Start()
    {
        mr = transform.GetComponent<SkinnedMeshRenderer>();
        if(PlayerPrefs.GetInt("player") == 1)
        {
            mr.material = skins[0];
        }
        else
        {
            mr.material = skins[1];
        }
    }
}
