using System.Collections.Generic;
using UnityEngine;

namespace CharacterScript
{
    public class AttackHitBox : MonoBehaviour
    {
        public Collider hitBoxCollider;
        public int damage = 30;

        // แก้เป็น enum
        public string targetTag;
        public List<Collider> damageTargetList = new();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(targetTag) || damageTargetList.Contains(other)) return;

            if (other.TryGetComponent(out Character character))
            {
                character.ApplyDamage(damage);
            }

            damageTargetList.Add(other);
        }

        private void OnValidate()
        {
            hitBoxCollider = GetComponent<Collider>();
            hitBoxCollider.enabled = false;
        }

        public void OpenAttackHitBox()
        {
            damageTargetList.Clear();
            hitBoxCollider.enabled = true;
        }

        public void CloseAttackHitBox()
        {
            damageTargetList.Clear();
            hitBoxCollider.enabled = false;
        }
    }
}