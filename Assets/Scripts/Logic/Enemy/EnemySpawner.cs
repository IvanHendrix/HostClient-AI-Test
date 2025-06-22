using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Logic.Enemy
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private EnemyAI _enemyPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        
        [SerializeField] private float _spawnInterval = 5f;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                if (_spawnPoints.Length > 0)
                {
                    StartCoroutine(SpawnEnemiesLoop());
                }
            }
        }

        private IEnumerator SpawnEnemiesLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            EnemyAI enemy = Instantiate(_enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemy.Spawn();
        }
    }
}