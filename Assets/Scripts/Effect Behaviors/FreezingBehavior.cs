using UnityEngine;
using UnityEngine.InputSystem;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Freezing Behavior")]
    public class FreezingBehavior : EffectBehavior
    {
        [Space(10)]
        [Header("This effect settings")]
        [Tooltip("Number of times you have to use an input for the effect to be removed.")]
        [SerializeField] int interactionAmount = 5;
        [SerializeField] GameObject effect;

        EffectBehaviorComponent behaviorComponent;
        PlayerMovement playerMovement;
        InputActionReference inputRef;
        Animator anim;

        float originalAnimSpeed;
        int counter = 0;

        GameObject goEffect;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            effectSfx.Post(behaviorComponent.gameObject);

            this.behaviorComponent = behaviorComponent;
            playerMovement = behaviorComponent.GetComponent<PlayerMovement>();
            playerMovement.UnsubscribeToAction();
            inputRef = playerMovement.MoveAction;
            counter = interactionAmount;
            inputRef.action.performed += OnPerformedAction;
            anim = behaviorComponent.GetComponent<Animator>();
            originalAnimSpeed = anim.speed;
            anim.speed = 0;
            goEffect = Instantiate(effect, behaviorComponent.transform);
        }

        void OnPerformedAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log(counter);
                counter--;
                if (counter <= 0)
                {
                    Destroy(behaviorComponent);
                }
            }
        }

        private void OnDestroy()
        {
            inputRef.action.performed -= OnPerformedAction;
            playerMovement.SubscribeToAction();
            anim.speed = originalAnimSpeed;
            Destroy(goEffect);
        }
    }
}