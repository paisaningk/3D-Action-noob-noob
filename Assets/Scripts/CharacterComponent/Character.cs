using System.Collections;
using UnityEngine;

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
        public SkinnedMeshRenderer skinnedMeshRenderer;


        [Header("AnimatorHash")]
        protected readonly int airBorneAnimator = Animator.StringToHash("AirBorne");
        protected readonly int attackAnimator = Animator.StringToHash("Attack");
        private readonly int blink = Shader.PropertyToID("_blink");
        protected readonly int speedAnimator = Animator.StringToHash("Speed");
        private Health health;
        private MaterialPropertyBlock materialPropertyBlock;


        protected virtual void Start()
        {
            EnterState();
        }

        protected virtual void OnValidate()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();

            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            materialPropertyBlock = new MaterialPropertyBlock();

            skinnedMeshRenderer.GetPropertyBlock(materialPropertyBlock);
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

            StartCoroutine(MaterialBlink());

            health.ApplyDamage(damage);
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
    }
}