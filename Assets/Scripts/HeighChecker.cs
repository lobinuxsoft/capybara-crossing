using UnityEngine;
using UnityEngine.Events;

namespace CapybaraCrossing
{
    public class HeighChecker : MonoBehaviour
    {
        [SerializeField] float heighCheck = -0.2f;

        public UnityEvent onBeforeDestroy;

        private PlayerMovement player;

        private void Start()
        {
            player = transform.GetComponent<PlayerMovement>();
        }

        private void LateUpdate()
        {
            if(transform.position.y < heighCheck)
            {
                Destroy(gameObject);
            }                
        }

        private void OnDestroy() => onBeforeDestroy?.Invoke();
    }
}