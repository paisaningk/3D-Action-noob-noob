using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class PlayerVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;
        public ParticleSystem attack1;

        public void UpdateFootStep(bool state)
        {
            if (state)
            {
                footStep.Play();
            }
            else
            {
                footStep.Stop();
            }
        }

        public void PlayAttack1()
        {
            attack1.Play();
        }
    }
}