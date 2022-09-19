using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
public class UIAutoRatioFitter : MonoBehaviour
{
    [SerializeField] float aspectRatio = 0;
    [SerializeField] Vector2 screenSize = Vector2Int.zero;
    [SerializeField] AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.FitInParent;
    AspectRatioFitter aspectRatioFitter = default;

    RectTransform parentRectTransform = default;

    private void Start()
    {
        parentRectTransform = (RectTransform)transform.parent;
        screenSize = new Vector2(parentRectTransform.rect.width, parentRectTransform.rect.height);

        if (aspectRatio != screenSize.x / screenSize.y)
            aspectRatio = screenSize.x / screenSize.y;

        aspectRatioFitter = GetComponent<AspectRatioFitter>();

        aspectRatioFitter.aspectMode = aspectMode;
        aspectRatioFitter.aspectRatio = aspectRatio;
    }

    void LateUpdate()
    {
        screenSize = new Vector2(parentRectTransform.rect.width, parentRectTransform.rect.height);
        if (aspectRatio != screenSize.x / screenSize.y)
            aspectRatio = screenSize.x / screenSize.y;

        aspectRatioFitter.aspectMode = aspectMode;
        aspectRatioFitter.aspectRatio = aspectRatio;
    }
}
