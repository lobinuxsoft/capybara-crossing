using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectionHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float fillSpeed = 1;
    [SerializeField] Image fillImage;

    bool isHold;

    public UnityEvent onSelected;

    private void Update()
    {
        if(!isHold)
        {
            fillImage.fillAmount -= Time.deltaTime * fillSpeed;
        }
        else
        {
            fillImage.fillAmount += Time.deltaTime * fillSpeed;

            if (fillImage.fillAmount >= 1f)
            {
                onSelected?.Invoke();
                enabled = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) => isHold = true;

    public void OnPointerUp(PointerEventData eventData) => isHold = false;

    public void ResetAll()
    {
        fillImage.fillAmount = 0;
        isHold = false;
        enabled = true;
    }
}