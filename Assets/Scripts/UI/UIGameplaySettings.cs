using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameplaySettings : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button backButton;

    [Space(10)]
    [Header("Volume Slider")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Space(10)]
    [Header("Wwise Settings")]
    [SerializeField] AK.Wwise.Event clickSfx;
    [SerializeField] AK.Wwise.Event backSfx;
    [SerializeField] AK.Wwise.RTPC musicVolume;
    [SerializeField] AK.Wwise.RTPC sfxVolume;

    [Space(10)]
    [Header("Change Scene Setting")]
    [SerializeField] string mainmenuSceneName = "MainMenu";
    [SerializeField] Gradient fadeInGradient;
    [SerializeField] Gradient fadeOutGradient;

    UIPopup popup;

    float backupTime = 0;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();

        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        backButton.onClick.AddListener(ToMainMenu);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);

        LoadVolumes();
    }

    private void OnDestroy()
    {
        pauseButton.onClick.RemoveListener(Pause);
        resumeButton.onClick.RemoveListener(Resume);
        backButton.onClick.RemoveListener(ToMainMenu);

        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);
    }

    private void Pause() => OpenSettings();

    private void Resume() => CloseSettings();

    private void OpenSettings()
    {
        clickSfx.Post(this.gameObject);
        backupTime = Time.timeScale;
        Time.timeScale = 0;
        popup.Show(() => { pauseButton.gameObject.SetActive(false); });
    }

    private void CloseSettings()
    {
        clickSfx.Post(this.gameObject);
        Time.timeScale = backupTime;
        popup.Hide(() => { pauseButton.gameObject.SetActive(true); });
    }

    private void ToMainMenu()
    {
        backSfx.Post(this.gameObject);
        TimelineUITransition.Instance.FadeStart(fadeInGradient, fadeOutGradient, 1.25f, () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(mainmenuSceneName);
        });
    }

    private void LoadVolumes()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float value = PlayerPrefs.GetFloat("MusicVolume");
            musicSlider.SetValueWithoutNotify(value);
            musicVolume.SetGlobalValue(value);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float value = PlayerPrefs.GetFloat("SFXVolume");
            sfxSlider.SetValueWithoutNotify(value);
            sfxVolume.SetGlobalValue(value);
        }
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        musicVolume.SetGlobalValue(value);
    }

    public void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        sfxVolume.SetGlobalValue(value);
    }
}