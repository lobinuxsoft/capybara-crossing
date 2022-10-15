using System.Collections;
using UnityEngine;
using LootLocker.Requests;
using CryingOnionTools.ScriptableVariables;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILeaderboard : MonoBehaviour
{
    const int leaderboardID = 7834;

    [Header("High score variable")]
    [SerializeField] UIntVariable highScore;
    [SerializeField] Gradient scoreGradient;

    [Space(10)]
    [Header("Text Mesh Pro objects")]
    [SerializeField] TextMeshProUGUI scoreLabel;
    [SerializeField] TextMeshProUGUI playerNames;
    [SerializeField] TextMeshProUGUI playerScores;
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] Button mainmenuButton;

    [Space(10)]
    [Header("Change Scene Setting")]
    [SerializeField] string mainmenuSceneName = "MainMenu";
    [SerializeField] Gradient fadeInGradient;
    [SerializeField] Gradient fadeOutGradient;

    private void Awake()
    {
        scoreLabel.text = $"Sumit Your Score\n{highScore.Value}";
        mainmenuButton.onClick.AddListener(ToMainMenu);
        playerNameInputField.onSubmit.AddListener(SetPlayerName);

        mainmenuButton.gameObject.SetActive(false);

        StartCoroutine(SetupRoutine());
    }

    private void OnDestroy()
    {
        mainmenuButton.onClick.RemoveListener(ToMainMenu);
        playerNameInputField.onSubmit.RemoveListener(SetPlayerName);
    }

    private void ToMainMenu()
    {
        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            SceneManager.LoadScene(mainmenuSceneName);
        });
    }

    private void SetPlayerName(string playerName)
    {
        LootLockerSDKManager.SetPlayerName(playerName, (response) =>
        {
            if (response.success)
            {
                SubmitScore();
            }
            else
            {
                Debug.LogError($"Could not set player name: {response.Error}");
            }
        });
    }

    private void SubmitScore()
    {
        StartCoroutine(SubmitScoreRoutine((int)highScore.Value));
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return FetchTopHighscoresRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.LogError("Could not start session");
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;

        LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    string colorHex = ColorUtility.ToHtmlStringRGB(scoreGradient.Evaluate((float)i / members.Length));

                    tempPlayerNames += $"<color=#{colorHex}>{members[i].rank}. ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += $"{members[i].player.name}</color>\n";
                    }
                    else
                    {
                        tempPlayerNames += $"{members[i].player.id}</color>\n";
                    }

                    tempPlayerScores += $"<color=#{colorHex}>{members[i].score}</color>\n";
                }

                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.LogError($"Failed: {response.Error}");
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        playerNameInputField.interactable = false;

        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>
        {
            if (response.success)
            {
                done = true;
            }
            else
            {
                playerNameInputField.interactable = false;
                Debug.LogError($"Failed: {response.Error}");
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
        yield return FetchTopHighscoresRoutine();

        mainmenuButton.gameObject.SetActive(true);
        playerNameInputField.gameObject.SetActive(false);
        scoreLabel.gameObject.SetActive(false);
    }
}