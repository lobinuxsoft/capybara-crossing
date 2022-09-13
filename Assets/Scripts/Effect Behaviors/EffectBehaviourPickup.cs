using UnityEngine;

namespace CapybaraCrossing
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class EffectBehaviourPickup : MonoBehaviour
    {
        [SerializeField] protected int index;
        [SerializeField] protected EffectBehaviorList effectBehaviorList;

        public int Index
        {
            get => index;
            set => index = value;
        }

        public EffectBehaviorList EffectBehaviorList => effectBehaviorList;

        public abstract void OnTriggerEnter(Collider other);
    }
}