using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class PlayerController : NetworkBehaviour, IDamageable
    {
        private const string AxisX = "Horizontal";
        private const string AxisY = "Vertical";
        
        public NetworkVariable<int> Health = new(100);

        public bool IsAlive => Health.Value > 0;
        
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Button _shutdownButton;
        
        [SerializeField] private CharacterController _characterController;

        private Vector3 _input;

        private void Start()
        {
            _shutdownButton.onClick.AddListener(Shudown);
        }

        private void Shudown()
        {
            GameManager.Instance.OnShutDown();
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                _playerCamera.gameObject.SetActive(true);
            }
            
            PlayerRegistry.AllPlayers.Add(this);
        }

        public override void OnNetworkDespawn()
        {
            PlayerRegistry.AllPlayers.Remove(this);
        }

        private void Update()
        {
            if (IsOwner)
            {
                Vector3 inputDir = GetAxisValue();
                SendInputServerRpc(inputDir);
            }
        }

        private void FixedUpdate()
        {
            if (IsServer)
            {
                _characterController.SimpleMove(_input * _moveSpeed);
            }
        }

        public void TakeDamage(int damage)
        {
            Debug.Log("Hit!");
            
            if (IsOwner)
            {
                Health.Value = Mathf.Max(Health.Value - damage, 0);
            }
        }

        private static Vector3 GetAxisValue()
        {
            return new Vector3(Input.GetAxis(AxisX), 0, Input.GetAxis(AxisY));
        }

        [ServerRpc]
        private void SendInputServerRpc(Vector3 dir)
        {
            MovePlayer(dir);
        }

        private void MovePlayer(Vector3 dir)
        {
            _input = dir;
        }
    }
}