using UnityEngine;
using UnityEngine.Playables;

public class DestroyWhenTimelineEnds : MonoBehaviour
{
    [SerializeField] float directorSpeed = 1;
    PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.playableGraph.GetRootPlayable(0).SetSpeed(directorSpeed);
        director.stopped += OnDirectorStop;
    }

    private void OnDestroy()
    {
        director.stopped -= OnDirectorStop;
    }

    private void OnDirectorStop(PlayableDirector director)
    {
        if(this.director == director)
        {
            Destroy(gameObject);
        }
    }
}