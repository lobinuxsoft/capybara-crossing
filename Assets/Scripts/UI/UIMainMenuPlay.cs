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
        string nextScene = playersCount > 1 ? multyPlayerSceneName : singlePlayerSceneName;

        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            SceneManager.LoadScene(nextScene);
        });
    }

    private void BackToMenu()
    {
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