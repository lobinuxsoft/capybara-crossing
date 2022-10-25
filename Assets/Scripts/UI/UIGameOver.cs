using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CryingOnionTools.ScriptableVariables;

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
    [Header("Change Scene Setting")]
    [SerializeField] string mainmenuSceneName = "MainMenu";
    [SerializeField] string leaderboardSceneName = "Leaderboard";
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
        leaderboardButton.onClick.AddListener(ToLeaderboard);
        exitButton.onClick.AddListener(ToMainMenu);
    }

    public void Show()
    {
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

        popup.Show();
    }

    private void OnDestroy()
    {
        retryButton.onClick.RemoveListener(PlayAgain);
        leaderboardButton.onClick.RemoveListener(ToLeaderboard);
        exitButton.onClick.RemoveListener(ToMainMenu);
    }

    private void PlayAgain()
    {
        popup.Hide();

        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    private void ToLeaderboard()
    {
        popup.Hide();

        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            SceneManager.LoadScene(leaderboardSceneName);
        });
    }

    private void ToMainMenu()
    {
        popup.Hide();

        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            SceneManager.LoadScene(mainmenuSceneName);
        });
    }
}