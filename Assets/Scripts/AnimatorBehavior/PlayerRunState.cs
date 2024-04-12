using UnityEngine;
using UnityEngine.Animations;
using VFX;

namespace AnimatorBehavior
{
    public class PlayerRunState : StateMachineBehaviour
    {
        public PlayerVFXManager playerVfx;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!playerVfx)
            {
                playerVfx = animator.GetComponent<PlayerVFXManager>();
            }

            playerVfx.UpdateFootStep(true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerVfx.UpdateFootStep(false);
        }
    }
}