using System.Collections;
using UnityEngine;
using LootLocker.Requests;

public class LootLockerConnector : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response)=>
        {
            if (response.success)
            {
                Debug.Log("Client was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}