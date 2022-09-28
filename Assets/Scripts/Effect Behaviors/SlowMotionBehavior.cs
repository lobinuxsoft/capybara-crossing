using System.Threading.Tasks;
using UnityEngine;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Slow Motion Behavior")]
    public class SlowMotionBehavior : EffectBehavior
    {
        [Tooltip("Effect duration in seconds")]
        [SerializeField] float effectDuration = 5;
        EffectBehaviorComponent behaviorComponent;
        PlayerMovement playerMovement;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            Time.timeScale = 0.1f;
            this.behaviorComponent = behaviorComponent;
            playerMovement = behaviorComponent.GetComponent<PlayerMovement>();
            playerMovement.SlowMotion = true;
            StartCountdown(effectDuration);
        }

        private async void StartCountdown(float duration)
        {
            float lerp = 0;
            while (lerp < duration)
            {
                lerp += Time.unscaledDeltaTime;
                await Task.Yield();
            }

            Time.timeScale = 1;
            playerMovement.SlowMotion = false;
            Destroy(behaviorComponent);
        }
    }
}