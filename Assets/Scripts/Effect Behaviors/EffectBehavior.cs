using UnityEngine;

namespace CapybaraCrossing
{
    public abstract class EffectBehavior : ScriptableObject
    {
        [SerializeField] private Sprite effectIcon;

        public Sprite EffectIcon => effectIcon;

        public abstract void OnInit(EffectBehaviorComponent behaviorComponent);
    }
}