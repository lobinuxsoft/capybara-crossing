using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CryingOnionTools.ScriptableVariables;
using GooglePlayGames;

public class UIGameOver : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] UIntVariable p1Score;
    [SerializeField] UIntVariable p2Score;
    [SerializeField] UIntVariable highScore;
    [SerializeField] string singlePlayerSceneName = "Gameplay-Single";

    [Space(10), Header("Labels")]
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI score;

    [Space(10), Header("Buttons")]
    [SerializeField] Button retryButton;
    [SerializeField] Button leaderboardButton;
    [SerializeField] Button exitButton;

    [Space(10)]
    [Header("Wwise Settings")]
    [SerializeField] AK.Wwise.Event retrySfx;
    [SerializeField] AK.Wwise.Event clickSfx;
    [SerializeField] AK.Wwise.Event backSfx;

    [Space(10)]
    [Header("Change Scene Setting")]
    [SerializeField] string mainmenuSceneName = "MainMenu";
    [SerializeField] Gradient fadeInGradient;
    [SerializeField] Gradient fadeOutGradient;

    [Space(10)]
    [Header("GameObjects to turn off")]
    [SerializeField] GameObject[] turnOffObjects;

    UIPopup popup;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();

        retryButton.onClick.AddListener(PlayAgain);
        leaderboardButton.onClick.AddListener(ShowLeaderboard);
        exitButton.onClick.AddListener(ToMainMenu);
    }

    public void Show()
    {
        MusicManager.Instance.PlayGameoverMusic();

        highScore.Value = 0;

        if(SceneManager.GetActiveScene().name == singlePlayerSceneName)
        {
            highScore.Value = p1Score.Value;
            label.text = $"¡Nice try!";
            score.text = $"{highScore.Value}";
        }
        else if (p1Score.Value == p2Score.Value)
        {
            highScore.Value = p1Score.Value;
            label.text = $"¡Tied game!";
            score.text = $"{highScore.Value}";
        }
        else if (p1Score.Value > p2Score.Value)
        {
            highScore.Value = p1Score.Value;
            label.text = $"¡Player 1 Wins!";
            score.text = $"{highScore.Value}";
        }
        else
        {
            highScore.Value = p2Score.Value;
            label.text = $"¡Player 2 Wins!";
            score.text = $"{highScore.Value}";
        }

        for (int i = 0; i < turnOffObjects.Length; i++)
            turnOffObjects[i].gameObject.SetActive(false);

#if UNITY_ANDROID
        // Achievements
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_step_by_step, highScore.Value, (bool success) => { });

        // Leaderboard
        PlayGamesPlatform.Instance.ReportScore(highScore.Value, GPGSIds.leaderboard_capybara_board, (bool success) => { });
#endif

        popup.Show();
    }

    private void OnDestroy()
    {
        retryButton.onClick.RemoveListener(PlayAgain);
        leaderboardButton.onClick.RemoveListener(ShowLeaderboard);
        exitButton.onClick.RemoveListener(ToMainMenu);
    }

    private void PlayAgain()
    {
        retrySfx.Post(this.gameObject);

        popup.Hide();
        Time.timeScale = 1;
        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    private void ShowLeaderboard()
    {
        clickSfx.Post(this.gameObject);

        Time.timeScale = 1;

        #if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
        #endif
    }

    private void ToMainMenu()
    {
        backSfx.Post(this.gameObject);
        popup.Hide();
        Time.timeScale = 1;
        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            SceneManager.LoadScene(mainmenuSceneName);
        });
    }
}