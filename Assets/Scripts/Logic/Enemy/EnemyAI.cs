using Logic.Bullets;
using Logic.Enemy.Enum;
using Logic.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.Enemy
{
    [RequireComponent(typeof(NetworkObject))] [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI : NetworkBehaviour
    {
        [Header("Targeting")]
        [SerializeField] private float _targetingRange = 10f;
        [SerializeField] private float _shootInterval = 2f;
        [SerializeField] private NavMeshAgent _agent;
        
        [SerializeField] private NetworkObject _networkObject;


        [Header("Projectile")]
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _projectileSpeed = 10f;

        private float _shootTimer;

        private PlayerController _currentTarget;

        private EnemyState _currentState = EnemyState.Idle;

        private void Update()
        {
            if (!IsServer)
            {
                return;
            }

            SetState();
        }

        public void Spawn()
        {
            _networkObject.Spawn();
        }

        private void SetState()
        {
            switch (_currentState)
            {
                case EnemyState.Idle:
                    TryFindTarget();
                    break;

                case EnemyState.Targeting:
                    if (!IsValidTarget(_currentTarget))
                    {
                        _currentState = EnemyState.Idle;
                        _agent.isStopped = true;
                        return;
                    }

                    MoveToTarget();

                    _shootTimer -= Time.deltaTime;
                    if (_shootTimer <= 0f)
                    {
                        Shoot();
                        _shootTimer = _shootInterval;
                    }

                    break;
            }
        }

        private void TryFindTarget()
        {
            _currentTarget = PlayerRegistry.GetClosestValidPlayer(transform.position, _targetingRange);

            if (_currentTarget != null)
            {
                _currentState = EnemyState.Targeting;
                _shootTimer = _shootInterval;
                _agent.isStopped = false;
            }
        }

        private bool IsValidTarget(PlayerController player)
        {
            return player != null && player.IsSpawned && player.IsAlive;
        }

        private void MoveToTarget()
        {
            if (_agent.enabled && _currentTarget != null)
            {
                _agent.SetDestination(_currentTarget.transform.position);
            }
        }

        private void Shoot()
        {
            if (_projectilePrefab == null || _shootPoint == null) return;

            Projectile projectile = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
            projectile.Initialize(_projectileSpeed);
            projectile.Spawn();
        }
    }
}