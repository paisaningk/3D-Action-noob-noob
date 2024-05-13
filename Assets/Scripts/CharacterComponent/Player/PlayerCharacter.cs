using System;
using System.Collections;
using CharacterScript.Player;
using Item;
using UnityEngine;
using UnityEngine.VFX;
using VFX;

namespace CharacterComponent
{
    public class PlayerCharacter : Character
    {
        [Header("Component")]
        public CharacterController characterController;
        public Camera cam;
        public PlayerInput playerInput;
        public AnimationEventCharacter animationEventCharacter;
        public Vector3 impactOnCharacter;
        public VisualEffect healVFX;
        public PlayerVFXManager playerVFXManager;


        [Header("Vertical")] [Space]
        public bool isGrounded;
        public float verticalVelocity;
        public float gravity = -9.8f;

        [Header("Attack")] [Space]
        public float attackStartTime;
        public float attackSlideSpeed = 0.06f;
        public float attackSlideDuration = 0.4f;
        public float attackAnimationDuration;


        [Header("Slide")] [Space]
        public float slideSpeed = 9f;
        private readonly int slide = Animator.StringToHash("Slide");

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
                case CharacterState.Dead:
                    break;
                case CharacterState.Hit:
                    animator.SetTrigger(beingHit);
                    StartCoroutine(InvincibleTime());
                    break;
                case CharacterState.Slide:
                    animator.SetTrigger(slide);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            isEnter = true;
        }

        protected override void Loop()
        {
            moveVelocity = Vector3.zero;

            switch (currentState)
            {
                case CharacterState.Idle:
                    IdleStateLoop();
                    break;
                case CharacterState.Attack:
                    AttackStateLoop();
                    break;
                case CharacterState.Dead:
                    break;
                case CharacterState.Hit:
                    if (impactOnCharacter.magnitude > 0.2f)
                    {
                        moveVelocity = impactOnCharacter * Time.deltaTime;
                    }

                    impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);
                    break;
                case CharacterState.Slide:
                    moveVelocity = transform.forward * (slideSpeed * Time.deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ApplyGravity();

            characterController.Move(moveVelocity);
        }

        private void AttackStateLoop()
        {
            if (Time.time < attackStartTime + attackSlideDuration)
            {
                var timePassed = Time.time - attackStartTime;
                var lerpTime = timePassed / attackSlideDuration;

                moveVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
            }

            if (playerInput.isMousePressed && characterController.isGrounded)
            {
                var currentClipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                attackAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (currentClipName != "LittleAdventurerAndie_ATTACK_03" && attackAnimationDuration > 0.5f &&
                    attackSlideDuration < 0.7f)
                {
                    playerInput.isMousePressed = false;

                    ExitStateTo(CharacterState.Attack);
                }
            }
        }

        private void IdleStateLoop()
        {
            if (playerInput.isMousePressed && characterController.isGrounded)
            {
                ExitStateTo(CharacterState.Attack);
                return;
            }

            if (playerInput.isSpaceKeyPressed && characterController.isGrounded)
            {
                ExitStateTo(CharacterState.Slide);
                return;
            }

            CalculateMovement();

            animator.SetBool(airBorneAnimator, !isGrounded);
        }

        protected override void ExitStateTo(CharacterState newState)
        {
            switch (newState)
            {
                case CharacterState.Idle:
                    break;
                case CharacterState.Attack:
                    animationEventCharacter.CloseAttackHitBox();
                    playerVFXManager.StopAllAttackVFX();
                    break;
                case CharacterState.Dead:
                    break;
                case CharacterState.Hit:
                    break;
                case CharacterState.Slide:
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

        public void HitAnimationEnd()
        {
            ExitStateTo(CharacterState.Idle);
        }

        public void SlideAnimationEnd()
        {
            ExitStateTo(CharacterState.Idle);
        }

        public override void ApplyDamage(int damage, Vector3 attackerPos = new())
        {
            base.ApplyDamage(damage, attackerPos);

            if (isDead)
            {
                return;
            }

            AddImpact(attackerPos, 10);
            ExitStateTo(CharacterState.Hit);
        }

        private IEnumerator InvincibleTime()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibleDuration);
            isInvincible = false;
        }

        private void AddImpact(Vector3 attack, float force)
        {
            var impactDir = transform.position - attack;
            impactDir.Normalize();
            impactDir.y = 0;
            impactOnCharacter = impactDir * force;
        }

        public override void PickUp(ItemPickUp itemPickUp)
        {
            base.PickUp(itemPickUp);

            switch (itemPickUp.pickUpType)
            {
                case PickUpType.Heal:
                    healVFX.Play();
                    break;
                case PickUpType.Coin:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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