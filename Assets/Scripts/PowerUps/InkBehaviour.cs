using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InkBehaviour : PowerUp
{
    private Vignette vig;
    private void Start()
    {
        Volume vol = FindObjectOfType<Volume>();
        
        if(vol.profile.TryGet<Vignette>(out vig))
        {
            vig.intensity.value = 1;
        }
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float time = 0;
        while (time < 5)
        {
            time += Time.deltaTime;
            yield return null;
        }
        vig.intensity.value = 0;
        Destroy(this);
    }
}
