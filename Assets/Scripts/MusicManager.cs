using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    [Header("Wwise settings")]
    [SerializeField] AK.Wwise.Bank bank;
    [SerializeField] AK.Wwise.Event musicEvent;

    [SerializeField] AK.Wwise.Switch mainmenuSwitch;
    [SerializeField] AK.Wwise.Switch gameoverSwitch;

    [SerializeField] AK.Wwise.Switch gameplayLayer1Switch;
    [SerializeField] AK.Wwise.Switch gameplayLayer2Switch;
    [SerializeField] AK.Wwise.Switch gameplayLayer3Switch;
    [SerializeField] AK.Wwise.Switch gameplayLayer4Switch;

    [SerializeField] int scoreToPlayLayer4;
    [SerializeField] int scoreToPlayLayer3;
    [SerializeField] int scoreToPlayLayer2;

    public static MusicManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            bank.Load();
            musicEvent.Post(this.gameObject);
        }
    }

    public void PlayMainMenuMusic()
    {
        mainmenuSwitch.SetValue(this.gameObject);
    }

    public void PlayGameoverMusic()
    {
        gameoverSwitch.SetValue(this.gameObject);
    }

    public void PlayGameplayMusic()
    {
        gameplayLayer1Switch.SetValue(this.gameObject);
    }

    public void EvaluateGameplayMusic(uint score)
    {
        if (score == scoreToPlayLayer4)
            gameplayLayer4Switch.SetValue(this.gameObject);
        else if (score == scoreToPlayLayer3)
            gameplayLayer3Switch.SetValue(this.gameObject);
        else if (score == scoreToPlayLayer2)
            gameplayLayer2Switch.SetValue(this.gameObject);
    }
}