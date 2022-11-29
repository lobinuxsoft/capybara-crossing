using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineUITransition : MonoBehaviour
{
    private static TimelineUITransition instance;

    public static TimelineUITransition Instance
    {
        get
        {
            if (instance == null)
                instance = Instantiate(Resources.Load<TimelineUITransition>(nameof(TimelineUITransition)));

            return instance;
        }
    }

    [SerializeField] TimelineAsset fadeIn;
    [SerializeField] TimelineAsset fadeOut;
    [SerializeField] Gradient fadeInGradient;
    [SerializeField] Gradient fadeOutGradient;
    [SerializeField] Image backgroundImage;


    PlayableDirector director;
    private UnityAction onFadeInEndAction = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        director = GetComponent<PlayableDirector>();
    }

    /// <summary>
    /// Comienza la animacion del Fade y se le puede pasar un metodo como parametro
    /// </summary>
    /// <param name="action"></param>
    IEnumerator FadeStart(float speed = 1f, UnityAction action = null)
    {
        onFadeInEndAction = action;

        director.Play(fadeOut);
        director.playableGraph.GetRootPlayable(0).SetSpeed(speed);

        while (director.time < director.duration)
        {
            backgroundImage.color = fadeInGradient.Evaluate((float)(director.time / director.duration));
            yield return null;
        }

        onFadeInEndAction?.Invoke();
        yield return new WaitForEndOfFrame();

        director.Play(fadeIn);
        director.playableGraph.GetRootPlayable(0).SetSpeed(speed);

        while (director.time < director.duration)
        {
            backgroundImage.color = fadeOutGradient.Evaluate((float)(director.time / director.duration));
            yield return null;
        }
    }

    public void FadeStart(Gradient fadeInGradient, Gradient fadeOutGradient, float speed = 1f, UnityAction action = null)
    {
        this.fadeInGradient = fadeInGradient;
        this.fadeOutGradient = fadeOutGradient;

        StartCoroutine(FadeStart(speed, action));
    }
}