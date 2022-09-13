using UnityEngine;
using UnityEngine.Playables;

public class FrameRateModifier : MonoBehaviour
{
    [Tooltip("-1 la plataforma destino usa su default frame rate")]
    [SerializeField, Min(-1)] int frameRate = 60;

    private void Awake() => SetFrameRate(frameRate);

    public void SetFrameRate(int targetFrameRate)
    {
        targetFrameRate = targetFrameRate > Screen.currentResolution.refreshRate ? Screen.currentResolution.refreshRate : targetFrameRate;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;

        Debug.Log($"Frame Rate: {Application.targetFrameRate} Hz");
    }
}