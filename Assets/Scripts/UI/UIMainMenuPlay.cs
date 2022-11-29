using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenuPlay : MonoBehaviour
{
    [Header("Players selection")]
    [SerializeField] UISelectionHold p1Hold;
    [SerializeField] UISelectionHold p2Hold;

    [Space(10)]
    [Header("Popups")]
    [SerializeField] UIPopup mainmenuPopup;

    [Space(10)]
    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button backButton;

    [Space(10)]
    [Header("Change Scene Setting")]
    [SerializeField] string singlePlayerSceneName = "Gameplay-Single";
    [SerializeField] string multyPlayerSceneName = "Gameplay-Multi";
    [SerializeField] Gradient fadeInGradient;
    [SerializeField] Gradient fadeOutGradient;

    [Space(10)]
    [Header("Wwise Settings")]
    [SerializeField] AK.Wwise.Event playSfx;
    [SerializeField] AK.Wwise.Event backSfx;

    int playersCount = 0;
    UIPopup popup;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();

        p1Hold.onSelected.AddListener(Selected);
        p2Hold.onSelected.AddListener(Selected);

        playButton.onClick.AddListener(ToGameplay);
        backButton.onClick.AddListener(BackToMenu);

        playButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        p1Hold.onSelected.RemoveListener(Selected);
        p2Hold.onSelected.RemoveListener(Selected);

        playButton.onClick.RemoveListener(ToGameplay);
        backButton.onClick.RemoveListener(BackToMenu);
    }

    private void ToGameplay()
    {
        playSfx.Post(this.gameObject);

        string nextScene = playersCount > 1 ? multyPlayerSceneName : singlePlayerSceneName;

        if(playersCount == 1 && p1Hold.IsSelected)
        {
            PlayerPrefs.SetInt("player", 1);
        }
        else if (playersCount == 1 && p2Hold.IsSelected)
        {
            PlayerPrefs.SetInt("player", 2);
        }

        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            SceneManager.LoadScene(nextScene);
        });
    }

    private void BackToMenu()
    {
        backSfx.Post(this.gameObject);
        popup.Hide(null, ResetAll);
        mainmenuPopup.Show();
    }

    private void Selected()
    {
        playersCount++;

        playButton.gameObject.SetActive(true);
    }

    private void ResetAll()
    {
        p1Hold.ResetAll();
        p2Hold.ResetAll();

        playersCount = 0;

        playButton.gameObject.SetActive(false);
    }
}