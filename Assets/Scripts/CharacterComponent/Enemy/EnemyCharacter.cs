using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using VFX;

namespace CharacterComponent
{
    public class EnemyCharacter : Character
    {
        [Header("Component")]
        public NavMeshAgent navMeshAgent;
        public Transform playerTransform;
        public EnemyVFXManager enemyVFXManager;
        public DropItem dropItem;

        private void FixedUpdate()
        {
            Loop();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            enemyVFXManager = GetComponent<EnemyVFXManager>();
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
                case CharacterState.Dead:
                    break;
                case CharacterState.Hit:
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
                case CharacterState.Dead:
                    break;
                case CharacterState.Hit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void ApplyDamage(int damage, Vector3 attackerPos = new())
        {
            base.ApplyDamage(damage, attackerPos);

            if (isDead)
            {
                return;
            }

            enemyVFXManager.PlayBeingHitVFX(attackerPos);
        }

        public void AttackAnimationEnd()
        {
            ExitStateTo(CharacterState.Idle);
        }

        protected override void CalculateMovement()
        {
            if (!playerTransform)
            {
                return;
            }

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

        protected override IEnumerator MaterialDissolve()
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

            dropItem.Drop();

            Destroy(gameObject);
        }

        public void RotateToTarget()
        {
            if (currentState != CharacterState.Dead)
            {
                transform.LookAt(playerTransform, Vector3.up);
            }
        }
    }
}