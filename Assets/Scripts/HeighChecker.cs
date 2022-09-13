using UnityEngine;
using UnityEngine.Events;

namespace CapybaraCrossing
{
    public class HeighChecker : MonoBehaviour
    {
        [SerializeField] float heighCheck = -5;

        public UnityEvent onBeforeDestroy;

        private void LateUpdate()
        {
            if(transform.position.y < heighCheck)
                Destroy(gameObject);
        }

        private void OnDestroy() => onBeforeDestroy?.Invoke();
    }
}