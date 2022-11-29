using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UINotificationMessage : MonoBehaviour
{
    [SerializeField] AnimationCurve fadeInBehavior;
    [SerializeField] AnimationCurve fadeOutBehavior;

    [SerializeField] Anchor startAnchor = new Anchor { minAnchor = new Vector2(-1, 0), maxAnchor = new Vector2(0, 1) };
    [SerializeField] Anchor middleAnchor = new Anchor { minAnchor = Vector2.zero, maxAnchor = Vector2.one };
    [SerializeField] Anchor endAnchor = new Anchor { minAnchor = new Vector2(1, 0), maxAnchor = new Vector2(2, 1) };

    [SerializeField] TextMeshProUGUI textMesh;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = (RectTransform)textMesh.transform;

        rectTransform.anchorMin = startAnchor.minAnchor;
        rectTransform.anchorMax = startAnchor.maxAnchor;
        textMesh.text = "";

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator ShowNotification(string message, Action endAction, float duration)
    {
        float t1 = duration * .25f;
        float t2 = duration * .5f;

        transform.SetAsFirstSibling();
        textMesh.text = message;
        yield return StartCoroutine(LerpRoutine(middleAnchor, fadeInBehavior, 1, t1));
        yield return new WaitForSecondsRealtime(t2);
        yield return StartCoroutine(LerpRoutine(endAnchor, fadeOutBehavior, 0, t1));

        rectTransform.anchorMin = startAnchor.minAnchor;
        rectTransform.anchorMax = startAnchor.maxAnchor;
        endAction?.Invoke();
    }

    IEnumerator LerpRoutine(Anchor anchor, AnimationCurve behavior, float toAlpha, float duration)
    {
        float lerp = 0;
        float oa = canvasGroup.alpha;

        while (lerp < duration)
        {
            lerp += Time.unscaledDeltaTime;

            rectTransform.anchorMin = Vector2.LerpUnclamped(rectTransform.anchorMin, anchor.minAnchor, behavior.Evaluate(Mathf.Clamp01(lerp / duration)));
            rectTransform.anchorMax = Vector2.LerpUnclamped(rectTransform.anchorMax, anchor.maxAnchor, behavior.Evaluate(Mathf.Clamp01(lerp / duration)));
            canvasGroup.alpha = Mathf.Lerp(oa, toAlpha, behavior.Evaluate(Mathf.Clamp01(lerp / duration)));

            yield return null;
        }

        rectTransform.anchorMin = anchor.minAnchor;
        rectTransform.anchorMax = anchor.maxAnchor;
        canvasGroup.alpha = toAlpha;
    }
}

[System.Serializable]
public struct Anchor
{
    public Vector2 minAnchor;
    public Vector2 maxAnchor;
}