using CharacterScript.Player;
using UnityEngine;

namespace CharacterScript
{
    public class Character : MonoBehaviour
    {
        [Header("Component")]
        public CharacterController characterController;
        public Camera cam;
        public Animator animator;
        public PlayerInput playerInput;

        [Header("Move")]
        public float moveSpeed = 5;
        public Vector3 moveVelocity;

        [Header("Vertical")]
        public float verticalVelocity;
        public float gravity = -9.8f;
        private readonly int airBorneAnimator = Animator.StringToHash("AirBorne");
        private readonly int speedAnimator = Animator.StringToHash("Speed");

        private void FixedUpdate()
        {
            CalculatePlayerMovement();

            if (!characterController.isGrounded)
            {
                verticalVelocity = gravity;
            }
            else
            {
                verticalVelocity = gravity * 0.3f;
            }

            animator.SetBool(airBorneAnimator, !characterController.isGrounded);

            moveVelocity += Vector3.up * (verticalVelocity * Time.deltaTime);

            characterController.Move(moveVelocity);
        }

        private void OnValidate()
        {
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();

            animator = GetComponent<Animator>();

            cam = Camera.main;
        }

        private Vector3 CalculatePlayerMovement()
        {
            moveVelocity = new Vector3(playerInput.directionMove.x, 0, playerInput.directionMove.y);
            moveVelocity = Quaternion.Euler(0, cam.transform.localRotation.y, 0) * moveVelocity;
            moveVelocity *= moveSpeed * Time.deltaTime;

            animator.SetFloat(speedAnimator, moveVelocity.magnitude);

            if (playerInput.directionMove != Vector2.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveVelocity);
            }

            return moveVelocity;
        }
    }
}