using System.Collections.Generic;
using System.Linq;
using CharacterComponent;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Spawn
{
    public class Spawner : MonoBehaviour
    {
        public List<EnemyCharacter> enemyInZone;
        public List<SpawnPoint> spawnPointList;
        public bool hasSpawn;
        public int enemyInSpawner;
        public Collider colliderToDraw;

        public UnityEvent onEnemyAllDead;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, colliderToDraw.bounds.size);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (hasSpawn)
            {
                return;
            }

            if (other.CompareTag("Player"))
            {
                SpawnEnemy();
            }
        }

        private void OnValidate()
        {
            if (!colliderToDraw)
            {
                colliderToDraw = GetComponent<Collider>();
            }
        }

        private void SpawnEnemy()
        {
            if (hasSpawn)
            {
                return;
            }

            hasSpawn = true;

            foreach (var spawnPoint in spawnPointList.Where(spawnPoint => spawnPoint.enemyToSpawn))
            {
                var instantiate = Instantiate(spawnPoint.enemyToSpawn, spawnPoint.transform.position,
                    quaternion.identity);

                instantiate.spawner = this;

                enemyInZone.Add(instantiate);
            }

            enemyInSpawner = enemyInZone.Count;
        }

        public void CheckEnemyDead()
        {
            enemyInSpawner--;

            if (enemyInSpawner <= 0)
            {
                onEnemyAllDead?.Invoke();
            }
        }

        [Button]
        public void AddSpawnList()
        {
            spawnPointList = new List<SpawnPoint>(transform.GetComponentsInChildren<SpawnPoint>());

            foreach (var spawnPoint in spawnPointList) spawnPoint.RenameGameObjectHaveEnemy();
        }
    }
}