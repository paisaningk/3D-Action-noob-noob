using System;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterComponent
{
    public class EnemyCharacter : Character
    {
        [Header("Component")]
        public NavMeshAgent navMeshAgent;
        public Transform playerTransform;

        private void FixedUpdate()
        {
            Loop();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = moveSpeed;

            if (!playerTransform)
            {
                playerTransform = FindObjectOfType<PlayerCharacter>().transform;
            }
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

                    transform.rotation = Quaternion.LookRotation(playerTransform.position - transform.position);
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
                    CalculateMovement();
                    break;
                case CharacterState.Attack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AttackAnimationEnd()
        {
            ExitStateTo(CharacterState.Idle);
        }

        protected override void CalculateMovement()
        {
            if (Vector3.Distance(playerTransform.position, transform.position) >= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.SetDestination(playerTransform.position);
                animator.SetFloat(speedAnimator, 0.2f);
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
                animator.SetFloat(speedAnimator, 0f);

                ExitStateTo(CharacterState.Attack);
            }
        }
    }
}