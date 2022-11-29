using UnityEngine;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Teleport Behavior")]
    public class TeleportBehavior : EffectBehavior
    {
        [SerializeField] float minDistance = 4;
        [SerializeField] float maxDistance = 10;
        [SerializeField] Vector3 teleportDirection = Vector3.forward;
        [SerializeField] ShurikenBakedMeshEmitter effectPref;

        ShurikenBakedMeshEmitter effect;
        EffectBehaviorComponent behaviorComponent;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            this.behaviorComponent = behaviorComponent;

            if(this.behaviorComponent.TryGetComponent(out PlayerMovement movement))
            {
                effect = Instantiate(effectPref, this.behaviorComponent.transform);
                effect.Skin = this.behaviorComponent.GetComponentInChildren<SkinnedMeshRenderer>();
                effect.Emit = true;
                Vector3 destination = this.behaviorComponent.transform.position + (teleportDirection * Random.Range(minDistance, maxDistance));
                movement.StopAllCoroutines();
                movement.StartCoroutine(movement.JumpRoutine(destination, .5f, () => StopEffect()));
            }
            else
            {
                Debug.LogWarning("No tiene el componente!!!");
                Destroy(this.behaviorComponent);
            }
        }

        private void StopEffect()
        {
            effect.Emit = false;
            Destroy(effect.gameObject);
            Destroy(this.behaviorComponent);
        }
    }
}