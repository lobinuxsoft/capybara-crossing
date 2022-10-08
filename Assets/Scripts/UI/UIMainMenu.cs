using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(UIPopup))]
public class UIMainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;

    [Space(10)]
    [Header("Popups")]
    [SerializeField] UIPopup playPopup;
    [SerializeField] UIPopup creditsPopup;
    [SerializeField] UIPopup settingsPopup;

    UIPopup popup;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();

        playButton.onClick.AddListener(ToGamePlay);
        creditsButton.onClick.AddListener(ToCredits);
        settingsButton.onClick.AddListener(ToSettings);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(ToGamePlay);
        creditsButton.onClick.RemoveListener(ToCredits);
        settingsButton.onClick.RemoveListener(ToSettings);
        exitButton.onClick.RemoveListener(ExitGame);
    }

    private void ToGamePlay()
    {
        popup.Hide();
        playPopup.Show();
    }

    private void ToCredits()
    {
        popup.Hide();
        creditsPopup.Show();
    }

    private void ToSettings()
    {
        popup.Hide();
        settingsPopup.Show();
    }

    private void ExitGame() => popup.Hide(null, () => { Application.Quit(); });
}