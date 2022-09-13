using UnityEngine;

namespace CapybaraCrossing
{
    public abstract class EffectBehavior : ScriptableObject
    {
        public abstract void OnInit(EffectBehaviorComponent behaviorComponent);
    }
}