using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace CharacterComponent
{
    public class AttackHitBox : MonoBehaviour
    {
        public VisualEffect slash;
        public Collider hitBoxCollider;
        public int damage = 30;

        public TargetTag targetTag;
        public List<Collider> damageTargetList = new();

        private void OnDrawGizmos()
        {
            //var originalPos = transform.position + -hitBoxCollider.bounds.extents.z * transform.forward;
            // Gizmos.color = Color.green;
            // Gizmos.DrawSphere(originalPos, 0.1f);

            // var rayCast = RayCastTarget();
            //
            // if (!rayCast.isHit) return;
            //
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawSphere(rayCast.hit.point, 0.3f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(targetTag.ToString()) || damageTargetList.Contains(other)) return;

            if (other.TryGetComponent(out Character character))
            {
                character.ApplyDamage(damage, transform.parent.position);

                if (character.isDead)
                {
                    return;
                }

                var rayCast = RayCastTarget();

                if (rayCast.isHit)
                {
                    PlaySlash(rayCast.hit.point + new Vector3(0, 0.5f, 0));
                }
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

        private (bool isHit, RaycastHit hit) RayCastTarget()
        {
            var originalPos = transform.position + -hitBoxCollider.bounds.extents.z * transform.forward;

            var isHit = Physics.BoxCast(originalPos, hitBoxCollider.bounds.extents / 2, transform.forward,
                out var hit, transform.rotation, hitBoxCollider.bounds.extents.z, 1 << 6);

            return (isHit, hit);
        }

        public void PlaySlash(Vector3 position)
        {
            slash.transform.position = position;
            slash.Play();
        }
    }
}