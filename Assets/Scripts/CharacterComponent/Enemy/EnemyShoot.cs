using UnityEngine;

namespace CharacterComponent.Enemy
{
    public class EnemyShoot : MonoBehaviour
    {
        public Transform shootPoint;
        public GameObject damageOrb;

        public EnemyCharacter enemyCharacter;

        public void Update()
        {
            if (enemyCharacter.isDead)
            {
                return;
            }

            enemyCharacter.RotateToTarget();
        }

        //call in anim
        public void Shoot()
        {
            Instantiate(damageOrb, shootPoint.position, Quaternion.LookRotation(shootPoint.forward));
        }
    }
}