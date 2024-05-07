using System;
using CharacterComponent;
using UnityEngine;

namespace Item
{
    public class ItemPickUp : MonoBehaviour
    {
        public PickUpType pickUpType;
        public int value = 20;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out Character character))
                {
                    character.PickUp(this);
                }
                
                Destroy(gameObject);
            }
        }
    }
}