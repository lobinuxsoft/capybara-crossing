using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameplaySettings : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button pauseButton;
    [SerializeField] Button backButton;

    [Space(10)]
    [Header("Change Scene Setting")]
    [SerializeField] string mainmenuSceneName = "MainMenu";
    [SerializeField] Gradient fadeInGradient;
    [SerializeField] Gradient fadeOutGradient;

    UIPopup popup;

    float backupTime = 0;

    bool isPause = false;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();

        pauseButton.onClick.AddListener(Pause);
        backButton.onClick.AddListener(ToMainMenu);
    }

    private void Pause()
    {
        isPause = !isPause;

        if(isPause)
        {
            OpenSettings();
        }
        else
        {
            CloseSettings();
        }
    }

    private void OpenSettings()
    {
        backupTime = Time.timeScale;
        Time.timeScale = 0;
        popup.Show();
    }

    private void CloseSettings()
    {
        Time.timeScale = backupTime;
        popup.Hide();
    }

    private void ToMainMenu()
    {
        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(mainmenuSceneName);
        });
    }
}
