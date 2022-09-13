using UnityEngine;

public class TeleportBehaviour : PowerUp
{
    void Start()
    {
        user.GetComponent<PlayerMovement>().StopAllCoroutines();
        user.transform.position += new Vector3(0,0,Random.Range(4, 10));
        Destroy(this);
    }
}
