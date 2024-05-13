using Sirenix.OdinInspector;
using UnityEngine;

namespace Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        public GameObject enemyToSpawn;

        [Button]
        public void RenameGameObjectHaveEnemy()
        {
            var gameObject1 = gameObject;

            var replace = gameObject1.name.Replace(enemyToSpawn.name, string.Empty);

            replace = replace.Replace("/", string.Empty);
            replace = replace.Trim();

            gameObject1.name = $"{replace} / {enemyToSpawn.name}";
        }
    }
}