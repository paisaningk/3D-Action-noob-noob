using CharacterComponent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        public EnemyCharacter enemyToSpawn;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var center = transform.position + new Vector3(0, 0.5f);
            Gizmos.DrawWireCube(center, Vector3.one);
            Gizmos.DrawLine(center, center + transform.forward * 2);
        }

        [Button]
        public void RenameGameObjectHaveEnemy()
        {
            gameObject.name = $"SpawnPoint / {enemyToSpawn.name}";
        }
    }
}