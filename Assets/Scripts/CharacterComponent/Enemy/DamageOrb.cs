using CharacterComponent.Player;
using Unity.Mathematics;
using UnityEngine;

namespace Item
{
    public class DamageOrb : MonoBehaviour
    {
        public float speed = 2f;
        public int damage = 10;
        public ParticleSystem hitVfx;
        public Rigidbody rb;

        private void FixedUpdate()
        {
            rb.MovePosition(transform.position + transform.forward * (speed * Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter playerCharacter))
            {
                playerCharacter.ApplyDamage(damage);
            }

            Instantiate(hitVfx, transform.position, quaternion.identity);

            Destroy(gameObject);
        }
    }
}