using UnityEngine;

namespace CharacterComponent
{
    public class Health : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;
        public float CurrentHealthPercentage => (float)currentHealth / maxHealth;

        private void OnValidate()
        {
            currentHealth = maxHealth;
        }


        public void ApplyDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }

            Debug.Log(gameObject.name + "Took damage : " + damage + " Current Health : " + currentHealth);
        }
    }
}