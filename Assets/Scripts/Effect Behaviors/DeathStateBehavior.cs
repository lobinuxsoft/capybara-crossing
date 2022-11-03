using System.Threading.Tasks;
using UnityEngine;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Death State Behavior")]
    public class DeathStateBehavior : EffectBehavior
    {
        [Tooltip("Effect duration in seconds")]
        [SerializeField] float effectDuration = 10000;
        EffectBehaviorComponent behaviorComponent;
        PlayerMovement playerMovement;
        TMPro.TextMeshPro resurrectText;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            this.behaviorComponent = behaviorComponent;
            playerMovement = behaviorComponent.GetComponent<PlayerMovement>();
            playerMovement.IsDead = true;
            resurrectText = behaviorComponent.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshPro>();
            resurrectText.gameObject.SetActive(true);
            StartCountdown(effectDuration);
        }

        private async void StartCountdown(float duration)
        {
            float lerp = 0;
            while (lerp < duration)
            {
                if (!playerMovement.IsDead)
                {
                    resurrectText.gameObject.SetActive(false);
                    Destroy(behaviorComponent,0.2f);
                    break;
                }
                lerp += Time.unscaledDeltaTime;
                resurrectText.text = $"{effectDuration - lerp:0}";
                resurrectText.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                await Task.Yield();
            }
            if(playerMovement.IsDead)
                Destroy(playerMovement.gameObject);
        }
    }
}