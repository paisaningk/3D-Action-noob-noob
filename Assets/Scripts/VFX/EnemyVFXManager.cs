using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class EnemyVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;
        public VisualEffect attackVFX;

        // use in animator if event in animator same name in function , it will play
        public void PlayFootStep()
        {
            footStep.SendEvent("OnPlay");
        }

        public void PlayAttackVFX()
        {
            attackVFX.SendEvent("OnPlay");
        }
    }
}