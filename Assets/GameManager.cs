using Cinemachine;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIGameOver uIGameOver;

    private static List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<CinemachineBrain> cameraBrains = new List<CinemachineBrain>();
    [SerializeField] private Material shader;

    static public List<GameObject> Players
    {
        get
        {
            return players;
        }
    }

    private void Start()
    {
        shader.SetColor("_RedChannelColor", new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1));
        FindExistingPlayers();
    }

    private void FindExistingPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player").ToList();

        #if UNITY_ANDROID
        if (players.Count > 1)
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_eternal_rivals, 100.0, (bool success) => { });
        #endif

        cameraBrains = FindObjectsOfType<CinemachineBrain>().ToList();
    }

    public void RemovePlayer()
    {
        StartCoroutine(CheckPlayers());
        
    }

    IEnumerator CheckPlayers()
    {
        yield return new WaitForEndOfFrame();

        players = players.Where(p => p != null).ToList();

        if (players.Count <= 0)
        {
            Time.timeScale = 0;
            uIGameOver.Show();
        }

            yield return new WaitForEndOfFrame();
        StartCoroutine(CheckCameras());
    }

    IEnumerator CheckCameras()
    {
        yield return new WaitForEndOfFrame();

        if (cameraBrains.Count > 1 && players.Count > 0)
        {
            foreach (var cb in cameraBrains)
            {
                if (!cb.ActiveVirtualCamera.Follow)
                {
                    Destroy(cb.ActiveVirtualCamera.VirtualCameraGameObject);
                    Destroy(cb.gameObject);
                }
            }

            yield return new WaitForEndOfFrame();

            cameraBrains = cameraBrains.Where(c => c != null).ToList();

            if (cameraBrains.Count == 1)
            {
                StartCoroutine(LerpCameraRect(cameraBrains[0].OutputCamera));
            }
        }
    }

    IEnumerator LerpCameraRect(Camera camera, float duration = 1)
    {
        Vector2 XY = camera.rect.position;
        Vector2 Size = camera.rect.size;
        float lerp = 0;

        while (lerp < duration)
        {
            camera.rect = new Rect(
                Vector2.Lerp(XY, Vector2.zero, Mathf.Clamp01(lerp / duration)),
                Vector2.Lerp(Size, Vector2.one, Mathf.Clamp01(lerp / duration))
            );

            lerp += Time.deltaTime;
            yield return null;
        }

        camera.rect = new Rect(
            Vector2.Lerp(XY, Vector2.zero, Mathf.Clamp01(lerp / duration)),
            Vector2.Lerp(Size, Vector2.one, Mathf.Clamp01(lerp / duration))
        );
    }
}