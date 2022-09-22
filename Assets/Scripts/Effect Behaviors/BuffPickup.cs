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

        public void SetRandomEffect() => index = Random.Range(0, effectBehaviorList.GetBehaviorNames().Length);

        private void OnTriggerEnter(Collider other)
        {
            EffectBehaviorComponent effectBehaviorComponent = other.gameObject.AddComponent<EffectBehaviorComponent>();
            SetRandomEffect();
            effectBehaviorComponent.Behavior = effectBehaviorList.GetEffectBehaviorInstance(index);

            Destroy(this.gameObject);
        }
    }
}