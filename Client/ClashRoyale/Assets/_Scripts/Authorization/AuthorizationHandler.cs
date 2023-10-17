using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.Network.Scripts {
    public class AuthorizationHandler : MonoBehaviour {
        [Scene, SerializeField] private string _menuSceneName;

        private void Start() => NetworkBootstrap.Instance.Authorization.Success += Success;

        private void OnDestroy() => NetworkBootstrap.Instance.Authorization.Success -= Success;

        private void Success() => SceneManager.LoadScene(_menuSceneName);
    }
}