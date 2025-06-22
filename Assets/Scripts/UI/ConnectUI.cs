using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ConnectUI : MonoBehaviour
    {
        public event Action OnHostClick;
        public event Action OnClientClick;

        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;

        private void Start()
        {
            _hostButton.onClick.AddListener(OnStartHostButtonClick);
            _clientButton.onClick.AddListener(OnStartClientButtonClick);
        }

        private void OnStartHostButtonClick()
        {
            OnHostClick?.Invoke();
        }

        private void OnStartClientButtonClick()
        {
            OnClientClick?.Invoke();
        }

        private void OnDestroy()
        {
            _hostButton.onClick.RemoveListener(OnStartHostButtonClick);
            _clientButton.onClick.RemoveListener(OnStartClientButtonClick);
        }
    }
}