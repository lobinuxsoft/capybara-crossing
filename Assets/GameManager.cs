using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Gradient fadeIn;
    [SerializeField] Gradient fadeOut;
    [SerializeField] UIPopup popup;


    private List<GameObject> players = new List<GameObject>();

    private void Start()
    {
        FindExistingPlayers();
    }

    private void FindExistingPlayers()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
    }

    public void ChangeScene(string scene)
    {
        popup.Hide(()=> {
            TimelineUITransition.Instance.FadeStart(fadeIn, fadeOut, 1, () =>
            {
                SceneManager.LoadScene(scene);
            });
        });
    }

    public void RemovePlayer(Transform t)
    {
        players.Remove(t.gameObject);
        if(players.Count <= 0)
        {
            popup.Show();
        }
    }
}
