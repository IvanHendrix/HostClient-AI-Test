using Unity.Netcode;
using UnityEngine;

namespace Logic.Bullets
{
    [RequireComponent(typeof(NetworkObject))]
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _lifeTime = 5f;
        
        [SerializeField] private NetworkObject _networkObject;

        private float _speed;

        private void Start()
        {
            if (IsServer)
            {
                Destroy(gameObject, _lifeTime);
            }
        }

        private void Update()
        {
            if (IsServer)
            {
                Move();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
            {
                return;
            }

            if (other.TryGetComponent<IDamageable>(out var player))
            {
                player.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }

        public void Initialize(float projectileSpeed)
        {
            _speed = projectileSpeed;
        }

        public void Spawn()
        {
            _networkObject.Spawn();
        }

        private void Move()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
    }
}