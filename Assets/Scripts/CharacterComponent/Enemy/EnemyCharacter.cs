using System;
using System.Collections;
using CharacterComponent.Player;
using Spawn;
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

        [Header("Spawn")]
        public CharacterState starState = CharacterState.Spawn;
        public float currentSpawnTime;
        public float spawnDuration;
        public Spawner spawner;


        protected override void Start()
        {
            currentState = starState;

            base.Start();

            if (!playerTransform)
            {
                playerTransform = FindObjectOfType<PlayerCharacter>().transform;
            }
        }

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
                case CharacterState.Slide:
                    break;
                case CharacterState.Spawn:
                    isInvincible = true;
                    currentSpawnTime = spawnDuration;

                    StartCoroutine(MaterialAppear());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            isEnter = true;
        }

        protected override void Loop()
        {
            if (impactOnCharacter.magnitude > 0.2f)
            {
                moveVelocity = impactOnCharacter * Time.deltaTime;
            }

            impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);

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
                case CharacterState.Slide:
                    break;
                case CharacterState.Spawn:
                    currentSpawnTime -= Time.deltaTime;

                    if (currentSpawnTime <= 0)
                    {
                        ExitStateTo(CharacterState.Idle);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (currentState == CharacterState.Idle) return;

            characterController.Move(moveVelocity);
            moveVelocity = Vector3.zero;
        }

        protected override void ExitStateTo(CharacterState newState)
        {
            switch (currentState)
            {
                case CharacterState.Idle:
                    break;
                case CharacterState.Attack:
                    break;
                case CharacterState.Dead:
                    break;
                case CharacterState.Hit:
                    break;
                case CharacterState.Slide:
                    break;
                case CharacterState.Spawn:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.ExitStateTo(newState);
        }

        public override void ApplyDamage(int damage, Vector3 attackerPos = new())
        {
            base.ApplyDamage(damage, attackerPos);

            if (isDead || isInvincible)
            {
                return;
            }

            AddImpact(attackerPos, 5.5f);
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
            spawner.CheckEnemyDead();

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

        protected IEnumerator MaterialAppear()
        {
            var dissolveTimeDuration = spawnDuration;
            var currentDissolveTime = 0f;
            var dissolveHighStart = -10f;
            var dissolveHighTarget = 20f;

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

            isInvincible = false;

            materialPropertyBlock.SetFloat(enableDissolve, 0);
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
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