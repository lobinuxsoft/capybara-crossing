using UnityEngine;

public class SwapPlayersBehaviour : PowerUp
{
    void Start()
    {
        user.GetComponent<PlayerMovement>().StopAllCoroutines();
        affected.GetComponent<PlayerMovement>().StopAllCoroutines();
        Vector3 positionAux = user.transform.position;
        user.transform.position = affected.transform.position;
        affected.transform.position = positionAux;
        Destroy(this);
    }

}
