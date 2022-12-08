using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
