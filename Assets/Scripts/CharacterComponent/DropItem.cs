using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace CharacterComponent
{
    public class DropItem : MonoBehaviour
    {
        [SerializeField] private GameObject drop;

        [Button]
        public void Drop()
        {
            Instantiate(drop, transform.position, quaternion.identity);
        }
    }
}