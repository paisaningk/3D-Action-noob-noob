﻿using UnityEngine;

namespace CharacterComponent
{
    public abstract class Character : MonoBehaviour
    {
        [Header("Animator")]
        public Animator animator;

        [Header("Move")]
        public float moveSpeed = 5;
        public Vector3 moveVelocity;

        [Header("State")]
        public CharacterState currentState;
        public bool isEnter;

        [Header("AnimatorHash")]
        protected readonly int airBorneAnimator = Animator.StringToHash("AirBorne");
        protected readonly int attackAnimator = Animator.StringToHash("Attack");
        protected readonly int speedAnimator = Animator.StringToHash("Speed");
        protected Health health;

        protected virtual void Start()
        {
            EnterState();
        }

        protected virtual void OnValidate()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        protected abstract void EnterState();

        protected abstract void Loop();

        protected virtual void ExitStateTo(CharacterState newState)
        {
            currentState = newState switch
            {
                CharacterState.Idle => CharacterState.Idle,
                CharacterState.Attack => CharacterState.Attack,
                _ => currentState
            };

            isEnter = false;
            EnterState();

            Debug.Log("Exit");
        }

        public virtual void ApplyDamage(int damage, Vector3 attackerPos = new())
        {
            if (!health)
            {
                Debug.Log("Can't found health", gameObject);
            }

            health.ApplyDamage(damage);
        }

        protected virtual void CalculateMovement()
        {
        }
    }
}