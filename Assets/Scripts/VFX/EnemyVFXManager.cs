using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class EnemyVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;
        public VisualEffect attackVFX;
        public ParticleSystem beingHitVFX;
        public VisualEffect beingHitSplashVFX;

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
            var position = transform.position;
            var forceForward = position - attackerPos;
            forceForward.Normalize();
            forceForward.y = 0;

            beingHitVFX.transform.rotation = Quaternion.LookRotation(forceForward);
            beingHitVFX.Play();

            var splashPos = position;

            splashPos.y += 2;

            var newSplashVFX = Instantiate(beingHitSplashVFX, splashPos, quaternion.identity);
            newSplashVFX.Play();

            Destroy(newSplashVFX, 10f);
        }
    }
}