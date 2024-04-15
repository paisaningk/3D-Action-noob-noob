using UnityEngine;
using UnityEngine.Animations;
using VFX;

namespace AnimatorBehavior
{
    public class PlayerRunState : StateMachineBehaviour
    {
        public PlayerVFXManager playerVfx;
        public bool isGet;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!isGet && animator.TryGetComponent(out playerVfx))
            {
                isGet = true;
            }


            if (playerVfx)
            {
                playerVfx.UpdateFootStep(true);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (playerVfx)
            {
                playerVfx.UpdateFootStep(false);
            }
        }
    }
}