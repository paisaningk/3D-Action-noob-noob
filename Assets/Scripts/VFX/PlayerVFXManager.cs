using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class PlayerVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;
        public ParticleSystem attack1;
        public ParticleSystem attack2;
        public ParticleSystem attack3;

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
        
        public void PlayAttack2()
        {
            attack2.Play();
        }

        public void StopVFX()
        {
            attack1.Simulate(0);
            attack1.Stop();
            
            attack2.Simulate(0);
            attack2.Stop();
            
            attack3.Simulate(0);
            attack3.Stop();
        }
        
        public void PlayAttack3()
        {
            attack3.Play();
        }
    }
}