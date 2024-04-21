using UnityEngine;

namespace CharacterComponent
{
    public class AnimationEventCharacter : MonoBehaviour
    {
        public AttackHitBox attackHitBox;

        public void OpenAttackHitBox()
        {
            attackHitBox.OpenAttackHitBox();
        }

        public void CloseAttackHitBox()
        {
            attackHitBox.CloseAttackHitBox();
        }
    }
}