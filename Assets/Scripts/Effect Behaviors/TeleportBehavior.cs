using UnityEngine;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Teleport Behavior")]
    public class TeleportBehavior : EffectBehavior
    {
        [SerializeField] float minDistance = 4;
        [SerializeField] float maxDistance = 10;
        [SerializeField] Vector3 teleportDirection = Vector3.forward;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            behaviorComponent.GetComponent<PlayerMovement>().StopAllCoroutines();
            behaviorComponent.transform.position += teleportDirection * Random.Range(minDistance, maxDistance);
            Destroy(behaviorComponent);
        }
    }
}