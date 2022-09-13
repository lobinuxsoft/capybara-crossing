using UnityEngine;
using UnityEngine.InputSystem;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Freezing Behavior")]
    public class FreezingBehavior : EffectBehavior
    {
        [Tooltip("Number of times you have to use an input for the effect to be removed.")]
        [SerializeField] int interactionAmount = 5;

        EffectBehaviorComponent behaviorComponent;
        PlayerMovement playerMovement;
        InputActionReference inputRef;
        int counter = 0;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            this.behaviorComponent = behaviorComponent;
            playerMovement = behaviorComponent.GetComponent<PlayerMovement>();
            playerMovement.UnsubscribeToAction();
            inputRef = playerMovement.MoveAction;
            counter = interactionAmount;
            inputRef.action.performed += OnPerformedAction;
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
        }
    }
}