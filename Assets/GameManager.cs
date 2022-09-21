using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private List<GameObject> players = new List<GameObject>();
    [SerializeField] private GameObject titleLoseScreen;
    [SerializeField] private GameObject buttonsLoseScreen;

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
        SceneManager.LoadScene(scene);
    }

    public void RemovePlayer(Transform t)
    {
        players.Remove(t.gameObject);
        if(players.Count <= 0)
        {
            titleLoseScreen.SetActive(true);
            StartCoroutine(ShowLoseScreen());
        }
    }

    private IEnumerator ShowLoseScreen()
    {
        yield return new WaitForSeconds(0.5f);
        buttonsLoseScreen.SetActive(true);
    }
}
