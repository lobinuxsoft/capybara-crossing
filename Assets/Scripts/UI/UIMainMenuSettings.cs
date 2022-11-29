using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuSettings : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button backButton;

    [Space(10)]
    [Header("Volume Slider")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Space(10)]
    [Header("Popups")]
    [SerializeField] UIPopup mainmenuPopup;

    [Space(10)]
    [Header("Wwise Settings")]
    [SerializeField] AK.Wwise.Event backSfx;
    [SerializeField] AK.Wwise.RTPC musicVolume;
    [SerializeField] AK.Wwise.RTPC sfxVolume;

    UIPopup popup;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();
        backButton.onClick.AddListener(HidePopup);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);

        LoadVolumes();
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(HidePopup);
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);
    }

    private void HidePopup() 
    {
        backSfx.Post(this.gameObject);
        popup.Hide();
        mainmenuPopup.Show();
    }

    private void LoadVolumes()
    {
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            float value = PlayerPrefs.GetFloat("MusicVolume");
            musicSlider.SetValueWithoutNotify(value);
            musicVolume.SetGlobalValue(value);
        }

        if(PlayerPrefs.HasKey("SFXVolume"))
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