using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Ink Behavior")]
    public class InkBehavior : EffectBehavior
    {
        [Space(10)]
        [Header("This effect settings")]
        [Tooltip("Effect duration in seconds")]
        [SerializeField] float effectDuration = 5;

        EffectBehaviorComponent behaviorComponent;
        Volume[] volumes;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            effectSfx.Post(behaviorComponent.gameObject);

            volumes = FindObjectsOfType<Volume>()
                            .Where(x => x.transform.parent.gameObject.GetHashCode() != behaviorComponent.gameObject.GetHashCode()).ToArray();

            this.behaviorComponent = behaviorComponent;

            StartCountdown(effectDuration);
        }

        private async void StartCountdown(float seconds)
        {
            float lerp = 0;
            while (lerp < 1)
            {
                for (int i = 0; i < volumes.Length; i++)
                {
                    if (volumes[i].profile.TryGet(out Vignette vig))
                    {
                        vig.intensity.value = Mathf.Lerp(0, 1, lerp / 1);
                        await Task.Yield();
                    }
                }

                lerp += Time.unscaledDeltaTime;
            }

            lerp = 0;
            while (lerp < seconds)
            {
                lerp += Time.unscaledDeltaTime;
                await Task.Yield();
            }

            lerp = 0;
            while (lerp < 1)
            {
                for (int j = 0; j < volumes.Length; j++)
                {
                    if (volumes[j].profile.TryGet(out Vignette vig))
                    {
                        vig.intensity.value = Mathf.Lerp(1, 0, lerp / 1);
                        await Task.Yield();
                    }
                }

                lerp += Time.unscaledDeltaTime;
            }

            Destroy(behaviorComponent);
        }
    }
}