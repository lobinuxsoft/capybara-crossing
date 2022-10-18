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

        public static Action<int> OnJump;
        public static Action OnDeath;

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
            moveAction.action.Disable();
            moveAction.action.performed -= JumpToDirection;
            OnDeath();
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
            if(collision.transform.CompareTag("Movable Obstacle"))
            {
                Destroy(this.gameObject);
            }
        }

        public void JumpToDirection(InputAction.CallbackContext context)
        {
            if (!context.performed || !groundDetector.OnGround) return;

            Vector2 input = context.ReadValue<Vector2>();

            // Se fija si el input fue un slide o un tap para decidir como moverse
            viewDir = input.sqrMagnitude > 0
                ? (Mathf.Abs(input.x) > Mathf.Abs(input.y)) ? new Vector3(input.x, 0, 0) : new Vector3(0, 0, input.y)
                : Vector3.forward;

            transform.rotation = Quaternion.LookRotation(viewDir, transform.up);

            Vector3 destination = rb.position + transform.forward;
            destination.x = Mathf.RoundToInt(destination.x);
            destination.z = Mathf.RoundToInt(destination.z);

            if(CheckCanJump(rb.position, (destination - rb.position).normalized, .5f))
            {
                StartCoroutine(JumpRoutine(destination, jumpDuration));

                OnJump?.Invoke((int)rb.position.z);
            }
        }

        bool CheckCanJump(Vector3 origin, Vector3 direction, float distance)
        {
            Ray ray = new Ray(origin, direction);

            return !Physics.SphereCast(ray, .5f, distance, obstacleMask);
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