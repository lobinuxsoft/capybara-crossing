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
            behaviorComponent.GetComponent<Rigidbody>().isKinematic = true;
            //behaviorComponent.GetComponent<Collider>().enabled = false;
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
                    if(behaviorComponent)
                    behaviorComponent.GetComponent<Rigidbody>().isKinematic = false;
                    resurrectText.gameObject.SetActive(false);
                    Destroy(behaviorComponent);
                }
                lerp += Time.unscaledDeltaTime;
                resurrectText.text = (effectDuration - lerp).ToString("F2");
                resurrectText.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                behaviorComponent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                await Task.Yield();
            }
            Destroy(playerMovement.gameObject);
        }
    }
}