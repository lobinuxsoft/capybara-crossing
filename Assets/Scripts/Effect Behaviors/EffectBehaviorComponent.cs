using UnityEngine;

namespace CapybaraCrossing
{
    public class EffectBehaviorComponent : MonoBehaviour
    {
        private EffectBehavior behavior;

        public EffectBehavior Behavior
        {
            get => behavior;
            set 
            {
                behavior = value;
                behavior.OnInit(this);
            }
        }

        private void OnDestroy() => Destroy(behavior);
    }
}