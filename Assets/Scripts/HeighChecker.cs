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
                if (GameManager.Players.Count > 1)
                {
                    if (transform.gameObject == GameManager.Players[0])
                    {
                        transform.position = GameManager.Players[1].transform.position;
                    }
                    else
                    {
                        transform.position = GameManager.Players[0].transform.position;
                    }
                    if (!player.IsDead)
                    {
                        transform.GetComponent<PlayerMovement>().AddDeathState();
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
                
                
        }

        private void OnDestroy() => onBeforeDestroy?.Invoke();
    }
}