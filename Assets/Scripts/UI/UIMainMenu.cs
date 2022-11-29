using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIPopup))]
public class UIMainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button achievementsButton;
    [SerializeField] Button leaderboardButton;
    [SerializeField] Button creditsButton;
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
        settingsButton.onClick.AddListener(ToSettings);
        achievementsButton.onClick.AddListener(ShowAchievements);
        leaderboardButton.onClick.AddListener(ShowLeaderboard);
        creditsButton.onClick.AddListener(ToCredits);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(ToGamePlay);
        settingsButton.onClick.RemoveListener(ToSettings);
        achievementsButton.onClick.RemoveListener(ShowAchievements);
        leaderboardButton.onClick.RemoveListener(ShowLeaderboard);
        creditsButton.onClick.RemoveListener(ToCredits);
        exitButton.onClick.RemoveListener(ExitGame);
    }

    private void Start()
    {
        #if UNITY_ANDROID
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        #endif
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


    private void ShowAchievements()
    {
        #if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowAchievementsUI();
        #endif
    }

    private void ShowLeaderboard()
    {
        #if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
        #endif
    }

    #if UNITY_ANDROID
    void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log($"Authentication statuc: {status.ToString()}");

        switch (status)
        {
            case SignInStatus.Success:
                PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_welcome_to_the_jungle, 100.0f, (bool success) => { });
                break;
        }
    }
    #endif
}