using System;
using System.Threading.Tasks;
using UnityEngine;

public class SimpleUIBoingBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 axisToUse = Vector3.one;
    [SerializeField] private AnimationCurve boingBehavior = default;

    private RectTransform rectTransform = default;

    void Awake() => rectTransform = (RectTransform)transform;

    public async void PlayBehaviour(float duration, Action action, float actionShotNormalizedTime)
    {
        float end = Time.unscaledTime + duration;

        while (Time.unscaledTime < end)
        {
            float delta = duration - (end - Time.unscaledTime);

            if(delta / duration > actionShotNormalizedTime)
                action?.Invoke();

            rectTransform.localScale = axisToUse * boingBehavior.Evaluate(delta / duration);
            await Task.Yield();
        }

        rectTransform.localScale = axisToUse * boingBehavior.Evaluate(1);
    }

    public async void PlayBehaviour(float duration)
    {
        float end = Time.unscaledTime + duration;

        while (Time.unscaledTime < end)
        {
            float delta = duration - (end - Time.unscaledTime);
            rectTransform.localScale = axisToUse * boingBehavior.Evaluate(delta / duration);
            await Task.Yield();
        }
        
        rectTransform.localScale = axisToUse * boingBehavior.Evaluate(1);
    }
}