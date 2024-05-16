using CharacterComponent.Player;
using Unity.Mathematics;
using UnityEngine;

namespace Item
{
    public class ItemPickUp : MonoBehaviour
    {
        public PickUpType pickUpType;
        public int value = 20;

        public ParticleSystem collectedVFX;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out PlayerCharacter character))
                {
                    character.PickUp(this);
                }

                if (collectedVFX)
                {
                    Instantiate(collectedVFX, transform.position, quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }
}