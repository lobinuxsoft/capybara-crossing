using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class UIPopup : MonoBehaviour
{
    [SerializeField] TimelineAsset showBehavior;
    [SerializeField] TimelineAsset hideBehavior;

    PlayableDirector director;

    private void Awake() => director = GetComponent<PlayableDirector>();

    public void Show(Action onStart = null, Action onCompleted = null, float speed = 1.5f)
    {
        onStart?.Invoke();
        StartCoroutine(PlayDirector(showBehavior, speed, onCompleted));
    }

    public void Hide(Action onStart = null, Action onCompleted = null, float speed = 1.5f)
    {
        onStart?.Invoke();
        StartCoroutine(PlayDirector(hideBehavior, speed, onCompleted));
    }

    IEnumerator PlayDirector(TimelineAsset timeline, float speed, Action onCompleteAction)
    {
        director.RebindPlayableGraphOutputs();
        director.RebuildGraph();
        director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        director.Play(timeline);

        yield return new WaitForSeconds((float)director.duration);

        onCompleteAction?.Invoke();
    }
}