﻿using System;
using System.Collections;
using Item;
using UnityEngine;

namespace CharacterComponent
{
    public abstract class Character : MonoBehaviour
    {
        [Header("Animator")]
        public Animator animator;
        public CharacterController characterController;

        [Header("Move")] [Space]
        public Vector3 impactOnCharacter;
        public float moveSpeed = 5;
        public Vector3 moveVelocity;

        [Header("State")] [Space]
        public CharacterState currentState;
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public bool isEnter;
        public bool isDead;

        [Header("Invincible")] [Space]
        public bool isInvincible;
        public float invincibleDuration = 3;
        public Health health;


        [Header("AnimatorHash")]
        protected readonly int airBorneAnimator = Animator.StringToHash("AirBorne");
        protected readonly int attackAnimator = Animator.StringToHash("Attack");
        protected readonly int beingHit = Animator.StringToHash("BeingHit");
        private readonly int blink = Shader.PropertyToID("_blink");
        private readonly int dead = Animator.StringToHash("Dead");
        protected readonly int dissolveHeight = Shader.PropertyToID("_dissolve_height");
        protected readonly int enableDissolve = Shader.PropertyToID("_enableDissolve");
        protected readonly int speedAnimator = Animator.StringToHash("Speed");
        protected MaterialPropertyBlock materialPropertyBlock;


        protected virtual void Start()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            skinnedMeshRenderer.GetPropertyBlock(materialPropertyBlock);

            EnterState();
        }

        protected virtual void OnValidate()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            characterController = GetComponent<CharacterController>();

            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        protected abstract void EnterState();

        protected abstract void Loop();

        protected virtual void ExitStateTo(CharacterState newState)
        {
            currentState = newState switch
            {
                CharacterState.Idle => CharacterState.Idle,
                CharacterState.Attack => CharacterState.Attack,
                CharacterState.Dead => CharacterState.Dead,
                CharacterState.Hit => CharacterState.Hit,
                CharacterState.Slide => CharacterState.Slide,
                CharacterState.Spawn => CharacterState.Spawn,
                _ => currentState
            };

            if (currentState == CharacterState.Dead)
            {
                animator.SetTrigger(dead);
                return;
            }

            isEnter = false;
            EnterState();

            Debug.Log("Exit");
        }

        public virtual void ApplyDamage(int damage, Vector3 attackerPos = new())
        {
            if (isInvincible)
            {
                return;
            }

            if (!health)
            {
                Debug.Log("Can't found health", gameObject);
            }

            if (isDead) return;

            StartCoroutine(MaterialBlink());

            health.ApplyDamage(damage);

            IsDead();
        }

        protected virtual void IsDead()
        {
            if (health.currentHealth != 0 || isDead) return;

            ExitStateTo(CharacterState.Dead);
            isDead = true;

            StartCoroutine(MaterialDissolve());
        }

        protected virtual void CalculateMovement()
        {
        }

        private IEnumerator MaterialBlink()
        {
            materialPropertyBlock.SetFloat(blink, 0.4f);
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);

            yield return new WaitForSeconds(0.2f);

            materialPropertyBlock.SetFloat(blink, 0);
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        protected virtual IEnumerator MaterialDissolve()
        {
            yield return new WaitForSeconds(2);

            var dissolveTimeDuration = 2f;
            var currentDissolveTime = 0f;
            var dissolveHighStart = 20f;
            var dissolveHighTarget = -10f;

            materialPropertyBlock.SetFloat(enableDissolve, 1);
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);

            while (currentDissolveTime < dissolveTimeDuration)
            {
                currentDissolveTime += Time.deltaTime;
                var dissolveHigh = Mathf.Lerp(dissolveHighStart, dissolveHighTarget,
                    currentDissolveTime / dissolveTimeDuration);

                materialPropertyBlock.SetFloat(dissolveHeight, dissolveHigh);
                skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);

                yield return null;
            }

            Destroy(gameObject);
        }

        public void AddImpact(Vector3 attack, float force)
        {
            var impactDir = transform.position - attack;
            impactDir.Normalize();
            impactDir.y = 0;
            impactOnCharacter = impactDir * force;
        }

        public virtual void PickUp(ItemPickUp itemPickUp)
        {
            switch (itemPickUp.pickUpType)
            {
                case PickUpType.Heal:
                    health.currentHealth += itemPickUp.value;
                    break;
                case PickUpType.Coin:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}