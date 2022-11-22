using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectionHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float fillSpeed = 1;
    [SerializeField] Sprite normalState;
    [SerializeField] Sprite selectedState;
    [SerializeField] Image avatar;
    [SerializeField] Image fillImage;

    private bool isSelected;


    public bool IsSelected
    {
        get { return isSelected; }
    }

    bool isHold;

    public UnityEvent onSelected;

    private void Update()
    {
        avatar.sprite = normalState;

        if (!isHold)
        {
            fillImage.fillAmount -= Time.deltaTime * fillSpeed;
        }
        else
        {
            fillImage.fillAmount += Time.deltaTime * fillSpeed;

            if (fillImage.fillAmount >= 1f)
            {
                onSelected?.Invoke();

                isSelected = true;
                avatar.sprite = selectedState;
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