using CharacterScript.Player;
using UnityEngine;

namespace CharacterScript
{
    public class PlayerCharacter : Character
    {
        [Header("Component")]
        public CharacterController characterController;
        public Camera cam;
        public PlayerInput playerInput;

        [Header("Vertical")]
        public float verticalVelocity;
        public float gravity = -9.8f;

        private void FixedUpdate()
        {
            CalculateMovement();

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

        protected override void OnValidate()
        {
            base.OnValidate();

            playerInput = GetComponent<PlayerInput>();
            characterController = GetComponent<CharacterController>();
            cam = Camera.main;
        }

        protected override void CalculateMovement()
        {
            moveVelocity = new Vector3(playerInput.directionMove.x, 0, playerInput.directionMove.y);
            moveVelocity = Quaternion.Euler(0, cam.transform.localRotation.y, 0) * moveVelocity;
            moveVelocity *= moveSpeed * Time.deltaTime;

            animator.SetFloat(speedAnimator, moveVelocity.magnitude);

            if (playerInput.directionMove != Vector2.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveVelocity);
            }
        }
    }
}