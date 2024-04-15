using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class EnemyVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;

        // use in animator if event in animator same name in function , it will play
        public void PlayFootStep()
        {
            footStep.SendEvent("OnPlay");
        }
    }
}