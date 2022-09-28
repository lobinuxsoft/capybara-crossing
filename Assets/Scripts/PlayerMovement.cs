using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;

namespace CapybaraCrossing
{
    [RequireComponent(typeof(Rigidbody), typeof(GroundDetector))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] InputActionReference moveAction;
        [SerializeField] float jumpDuration = 1;
        [SerializeField] AnimationCurve jumpHeightBehaviour;

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
        }

        public void SubscribeToAction()
        {
            moveAction.action.performed += JumpToDirection;
        }

        public void UnsubscribeToAction()
        {
            moveAction.action.performed -= JumpToDirection;
        }


        public void JumpToDirection(InputAction.CallbackContext context)
        {
            if (!context.performed || !groundDetector.OnGround) return;

            Vector2 input = context.ReadValue<Vector2>();

            if (input.sqrMagnitude > 0)
            {
                viewDir = (Mathf.Abs(input.x) > Mathf.Abs(input.y)) ? new Vector3(input.x, 0, 0) : new Vector3(0, 0, input.y);

                transform.rotation = Quaternion.LookRotation(viewDir, transform.up);

                Vector3 destination = rb.position + transform.forward;
                destination.x = Mathf.RoundToInt(destination.x);
                destination.z = Mathf.RoundToInt(destination.z);

                StartCoroutine(JumpRoutine(destination, jumpDuration));

                OnJump?.Invoke((int)rb.position.z);
            }
            else
            {
                viewDir = new Vector3(0, 0, 1);

                transform.rotation = Quaternion.LookRotation(viewDir, transform.up);

                Vector3 destination = rb.position + transform.forward;
                destination.x = Mathf.RoundToInt(destination.x);
                destination.z = Mathf.RoundToInt(destination.z);

                StartCoroutine(JumpRoutine(destination, jumpDuration));

                OnJump?.Invoke((int)rb.position.z);
            }
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