using UnityEngine;

namespace CapybaraCrossing
{
    public abstract class EffectBehavior : ScriptableObject
    {
        [SerializeField] private Sprite effectIcon;

        [Space(10)]
        [Header("Wwise Settings")]
        [SerializeField] protected AK.Wwise.Event effectSfx;

        public Sprite EffectIcon => effectIcon;

        public abstract void OnInit(EffectBehaviorComponent behaviorComponent);
    }
}