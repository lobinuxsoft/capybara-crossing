using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuSettings : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button backButton;

    [Space(10)]
    [Header("Popups")]
    [SerializeField] UIPopup mainmenuPopup;

    UIPopup popup;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();
        backButton.onClick.AddListener(HidePopup);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(HidePopup);
    }

    private void HidePopup() 
    {
        popup.Hide();
        mainmenuPopup.Show();
    }
}