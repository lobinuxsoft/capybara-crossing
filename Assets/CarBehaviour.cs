using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event enableSfx;
    [SerializeField] AK.Wwise.Event visibleSfx;
    [SerializeField] AK.Wwise.Event triggerEnterSfx;

    private void OnEnable()
    {
        enableSfx.Post(this.gameObject);
    }

    private void OnDisable()
    {
        enableSfx.Stop(this.gameObject, 1);
    }

    private void OnBecameVisible()
    {
        visibleSfx.Post(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerEnterSfx.Post(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}
