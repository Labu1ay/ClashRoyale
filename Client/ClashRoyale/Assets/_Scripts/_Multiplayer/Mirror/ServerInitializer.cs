using System;
using Mirror;
using ParrelSync;
using Plugins.Network.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Multiplayer.Mirror {
    public class ServerInitializer : MonoBehaviour {
#if UNITY_SERVER || UNITY_EDITOR
        [Scene, SerializeField] private string _menuScena;
        [Scene, SerializeField] private string _offlineServerScena;
        [SerializeField] private CustomNetworkManager _customNetworkManagerPrefab;

        private void Start() {
#if UNITY_SERVER
            StartServer();
#else
            StartClient();
#endif
        }

        private void StartServer() {
            if (NetworkManager.singleton == false) {
                var manager = Instantiate(_customNetworkManagerPrefab);
                manager.offlineScene = _offlineServerScena;
            }

            NetworkManager.singleton.StartServer();
        }

        private void StartClient() {
            if (ClonesManager.IsClone()) {
                string customArgument = ClonesManager.GetArgument();
                int.TryParse(customArgument, out int id);
                UserInfo.Instance.SetID(id);
                SceneManager.LoadScene(_menuScena);
            }
        }
#endif
    }
}