using UnityEngine;
using UnityEngine.AI;

namespace CharacterScript
{
    public class EnemyCharacter : Character
    {
        [Header("Component")]
        public NavMeshAgent navMeshAgent;
        public Transform playerTransform;

        private void FixedUpdate()
        {
            CalculateMovement();
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
            }
        }
    }
}