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
        int jumpHash = Animator.StringToHash("JumpTime");
        int onGroundHash = Animator.StringToHash("OnGround");

        [SerializeField] InputActionReference moveAction;
        [SerializeField] float jumpDuration = 1;
        [SerializeField] AnimationCurve jumpHeightBehaviour;
        [SerializeField] LayerMask obstacleMask;

        [Space(10)]
        [Header("Wwise Settings")]
        [SerializeField] AK.Wwise.Event jumpSfx;
        [SerializeField] AK.Wwise.Event stepsSfx;

        Rigidbody rb;
        Animator anim;
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

            anim = GetComponent<Animator>();

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

        private void LateUpdate()
        {
            anim.SetBool(onGroundHash, groundDetector.OnGround);
        }

        public void SubscribeToAction()
        {
            moveAction.action.performed += JumpToDirection;
        }

        public void UnsubscribeToAction()
        {
            moveAction.action.performed -= JumpToDirection;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Movable Obstacle"))
            {
                Destroy(gameObject);
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

            if (CheckCanJump(rb.position, (destination - rb.position).normalized, 1f) && destination.x >= -9 && destination.x <= 10)
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

        public IEnumerator JumpRoutine(Vector3 destination, float duration, Action endAction = null)
        {
            jumpSfx.Post(this.gameObject);

            float lerp = 0;
            float destHeight = destination.y;
            Vector3 startPosition = rb.position;

            while (lerp < duration)
            {
                lerp += slowMotion ? Time.unscaledDeltaTime : Time.deltaTime;
                Vector3 XZ = Vector3.Lerp(startPosition, destination, Mathf.Clamp01(lerp / duration));
                XZ.y = destHeight + jumpHeightBehaviour.Evaluate(Mathf.Clamp01(lerp / duration));

                anim.SetFloat(jumpHash, jumpHeightBehaviour.Evaluate(Mathf.Clamp01(lerp / duration)));

                rb.MovePosition(XZ);

                yield return null;
            }

            anim.SetFloat(jumpHash, 0f);
            anim.SetBool(onGroundHash, true);

            rb.MovePosition(destination);
            endAction?.Invoke();

            stepsSfx.Post(this.gameObject);
        }
    }
}