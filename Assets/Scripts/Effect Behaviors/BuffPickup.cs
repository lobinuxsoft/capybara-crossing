using UnityEngine;

namespace CapybaraCrossing
{
    [RequireComponent(typeof(SphereCollider))]
    public class BuffPickup : MonoBehaviour
    {
        [SerializeField] protected int index;
        [SerializeField] protected EffectBehaviorList effectBehaviorList;

        private void Awake()
        {
            SphereCollider sc = GetComponent<SphereCollider>();
            sc.isTrigger = true;
        }

        public int Index
        {
            get => index;
            set => index = value;
        }

        public EffectBehaviorList EffectBehaviorList => effectBehaviorList;

        private void OnTriggerEnter(Collider other)
        {
            EffectBehaviorComponent effectBehaviorComponent = other.gameObject.AddComponent<EffectBehaviorComponent>();
            effectBehaviorComponent.Behavior = effectBehaviorList.GetEffectBehaviorInstance(index);

            Destroy(this.gameObject);
        }
    }
}