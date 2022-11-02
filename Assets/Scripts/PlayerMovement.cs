using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor;

namespace CapybaraCrossing
{
    [RequireComponent(typeof(Rigidbody), typeof(GroundDetector))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] InputActionReference moveAction;
        [SerializeField] float jumpDuration = 1;
        [SerializeField] AnimationCurve jumpHeightBehaviour;
        [SerializeField] LayerMask obstacleMask;

        Rigidbody rb;
        GroundDetector groundDetector;
        Vector3 viewDir = Vector3.forward;

        [SerializeField] EffectBehavior effectBehavior;
        EffectBehaviorComponent effectBehaviorComponent;
        private bool isDead = false;

        private bool slowMotion;

        public InputActionReference MoveAction
        {
            get
            {
                return moveAction;
            }
        }

        public bool SlowMotion
        {
            get
            {
                return slowMotion;
            }
            set
            {
                slowMotion = value;
            }
        }

        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set
            {
                isDead = value;
            }
        }

        public static Action<int> OnJump;
        public static Action OnDeath;
        public static Action OnResurrectNeed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            groundDetector = GetComponent<GroundDetector>();

            moveAction.action.performed += JumpToDirection;
            moveAction.action.Enable();
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke();
            moveAction.action.Disable();
            moveAction.action.performed -= JumpToDirection;
        }

        public void SubscribeToAction()
        {
            moveAction.action.performed += JumpToDirection;
        }

        public void UnsubscribeToAction()
        {
            moveAction.action.performed -= JumpToDirection;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.CompareTag("Movable Obstacle") && !isDead)
            {
                EffectBehavior deathEffect = ScriptableObject.Instantiate(effectBehavior);
                effectBehaviorComponent = gameObject.AddComponent<EffectBehaviorComponent>();
                effectBehaviorComponent.Behavior = deathEffect;
                OnResurrectNeed();
                UINotificationManager.Instance.ShowMessage(
                    $"A resurrect item has spawned"
                );
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Resurrect") && isDead)
            {
                other.gameObject.SetActive(false);
                isDead = false;
            }
        }

        public void JumpToDirection(InputAction.CallbackContext context)
        {
            
            if (!isDead)
            {
                if (!context.performed || !groundDetector.OnGround) return;
            }
            else
            {
                bool onGround = false;
                RaycastHit hit;
                Ray ray = new Ray(transform.position, -transform.up);
                if (Physics.Raycast(ray, out hit, 0.2f))
                {
                    if (hit.collider)
                    {
                        onGround = true;
                    }
                }
                if (!context.performed || !onGround) return;
            }

            Vector2 input = context.ReadValue<Vector2>();

            // Se fija si el input fue un slide o un tap para decidir como moverse
            viewDir = input.sqrMagnitude > 0
                ? (Mathf.Abs(input.x) > Mathf.Abs(input.y)) ? new Vector3(input.x, 0, 0) : new Vector3(0, 0, input.y)
                : Vector3.forward;

            transform.rotation = Quaternion.LookRotation(viewDir, transform.up);

            Vector3 destination = rb.position + transform.forward;
            destination.x = Mathf.RoundToInt(destination.x);
            destination.z = Mathf.RoundToInt(destination.z);

            if(isDead || CheckCanJump(rb.position, (destination - rb.position).normalized, 1f) && destination.x >= -9 && destination.x <= 10)
            {
                StartCoroutine(JumpRoutine(destination, jumpDuration));

                OnJump?.Invoke((int)rb.position.z);
            }
        }

        bool CheckCanJump(Vector3 origin, Vector3 direction, float distance)
        {
            Ray ray = new Ray(origin, direction);

            return !Physics.SphereCast(ray, .4f, distance, obstacleMask);
        }

        IEnumerator JumpRoutine(Vector3 destination, float duration)
        {
            float lerp = 0;
            float destHeight = destination.y;
            Vector3 startPosition = rb.position;

            while (lerp < duration)
            {
                lerp += slowMotion ? Time.unscaledDeltaTime : Time.deltaTime;
                Vector3 XZ = Vector3.Lerp(startPosition, destination, Mathf.Clamp01(lerp / duration));
                XZ.y = destHeight + jumpHeightBehaviour.Evaluate(Mathf.Clamp01(lerp / duration));

                rb.MovePosition(XZ);

                yield return null;
            }

            rb.MovePosition(destination);
        }
    }
}