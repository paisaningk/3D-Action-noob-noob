using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class EnemyVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;
        public VisualEffect attackVFX;
        public ParticleSystem beingHitVFX;

        // use in animator if event in animator same name in function , it will play
        public void PlayFootStep()
        {
            footStep.SendEvent("OnPlay");
        }

        public void PlayAttackVFX()
        {
            attackVFX.SendEvent("OnPlay");
        }

        public void PlayBeingHitVFX(Vector3 attackerPos)
        {
            var forceForward = transform.position - attackerPos;
            forceForward.Normalize();
            forceForward.y = 0;

            beingHitVFX.transform.rotation = Quaternion.LookRotation(forceForward);
            beingHitVFX.Play();
        }
    }
}