using UnityEngine;

namespace CharacterScript
{
    public abstract class Character : MonoBehaviour
    {
        [Header("Animator")]
        public Animator animator;

        [Header("Move")]
        public float moveSpeed = 5;
        public Vector3 moveVelocity;
        protected readonly int airBorneAnimator = Animator.StringToHash("AirBorne");
        protected readonly int speedAnimator = Animator.StringToHash("Speed");


        protected virtual void OnValidate()
        {
            animator = GetComponent<Animator>();
        }

        protected virtual void CalculateMovement()
        {
        }
    }
}