using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Ink Behavior")]
    public class InkBehavior : EffectBehavior
    {
        [Tooltip("Effect duration in seconds")]
        [SerializeField] float effectDuration = 5;
        EffectBehaviorComponent behaviorComponent;
        private Vignette vig;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            this.behaviorComponent = behaviorComponent;

            Volume vol = FindObjectOfType<Volume>();

            if (vol.profile.TryGet<Vignette>(out vig))
            {
                vig.intensity.value = 1;
            }

            StartCountdown(effectDuration);
        }

        private async void StartCountdown(float seconds)
        {
            await Task.Delay(Mathf.RoundToInt(seconds * 1000));   // Se realiza esa multiplicacion porque son milisegundos

            vig.intensity.value = 0;
            Destroy(behaviorComponent);
        }
    }
}