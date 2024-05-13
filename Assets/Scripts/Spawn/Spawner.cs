using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace Spawn
{
    public class Spawner : MonoBehaviour
    {
        public List<GameObject> enemyInZone;
        public List<SpawnPoint> spawnPointList;
        public bool hasSpawn;
        public Collider colliderToDraw;

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
                enemyInZone.Add(
                    Instantiate(spawnPoint.enemyToSpawn, spawnPoint.transform.position, quaternion.identity));
        }

        [Button]
        public void AddSpawnList()
        {
            spawnPointList = new List<SpawnPoint>(transform.GetComponentsInChildren<SpawnPoint>());

            foreach (var spawnPoint in spawnPointList) spawnPoint.RenameGameObjectHaveEnemy();
        }
    }
}