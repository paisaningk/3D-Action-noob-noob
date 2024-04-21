using System;
using CharacterScript.Player;
using UnityEngine;

namespace CharacterComponent
{
    public class PlayerCharacter : Character
    {
        [Header("Component")]
        public CharacterController characterController;
        public Camera cam;
        public PlayerInput playerInput;
        public AnimationEventCharacter animationEventCharacter;


        [Header("Vertical")]
        public bool isGrounded;
        public float verticalVelocity;
        public float gravity = -9.8f;

        public float attackStartTime;
        public float attackSlideSpeed = 0.06f;
        public float attackSlideDuration = 0.4f;

        private void FixedUpdate()
        {
            Loop();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            animationEventCharacter = GetComponent<AnimationEventCharacter>();
            playerInput = GetComponent<PlayerInput>();
            characterController = GetComponent<CharacterController>();
            cam = Camera.main;
        }

        protected override void EnterState()
        {
            if (isEnter)
            {
                return;
            }

            switch (currentState)
            {
                case CharacterState.Idle:
                    break;
                case CharacterState.Attack:
                    animator.SetTrigger(attackAnimator);
                    attackStartTime = Time.time;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            isEnter = true;
        }

        protected override void Loop()
        {
            switch (currentState)
            {
                case CharacterState.Idle:
                    IdleStateLoop();
                    break;
                case CharacterState.Attack:
                    AttackStateLoop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AttackStateLoop()
        {
            if (Time.time < attackStartTime + attackSlideDuration)
            {
                var timePassed = Time.time - attackStartTime;
                var lerpTime = timePassed / attackSlideDuration;

                moveVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
            }

            ApplyGravity();

            characterController.Move(moveVelocity);
        }

        private void IdleStateLoop()
        {
            if (playerInput.isMousePressed && characterController.isGrounded)
            {
                ExitStateTo(CharacterState.Attack);
                return;
            }

            CalculateMovement();

            ApplyGravity();

            animator.SetBool(airBorneAnimator, !isGrounded);

            characterController.Move(moveVelocity);
        }

        protected override void ExitStateTo(CharacterState newState)
        {
            switch (newState)
            {
                case CharacterState.Idle:
                    break;
                case CharacterState.Attack:
                    animationEventCharacter.CloseAttackHitBox();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            base.ExitStateTo(newState);
        }

        private void ApplyGravity()
        {
            if (!characterController.isGrounded)
            {
                verticalVelocity = gravity;
            }
            else
            {
                verticalVelocity = gravity * 0.3f;
            }

            isGrounded = characterController.isGrounded;
            moveVelocity += Vector3.up * (verticalVelocity * Time.deltaTime);
        }

        //call in animation attack
        public void AttackAnimationEnd()
        {
            ExitStateTo(CharacterState.Idle);
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