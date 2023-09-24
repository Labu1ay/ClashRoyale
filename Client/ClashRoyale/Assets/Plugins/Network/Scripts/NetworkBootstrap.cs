using System;
using UnityEngine;

namespace Plugins.Network.Scripts {
    public class NetworkBootstrap : MonoBehaviour {
        public static NetworkBootstrap Instance { get; private set; }
        
        public Network Network { get; private set; }
        public Authorization Authorization { get; private set; }
        public Registration Registration { get; private set; }
        
        private void Awake() {
#region Singlton
            if (Instance) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
#endregion
            ServiceRegistration();
            InstantiateCanvas();
            
        }

        private void ServiceRegistration() {
            Network = new Network();
            Authorization = new Authorization();
            Registration = new Registration();
        }

        private void InstantiateCanvas() {
            AuthorizationUI authorizationUIPrefab = Resources.Load<AuthorizationUI>("Canvas_Authorization");
            AuthorizationUI authorizationUI = Instantiate(authorizationUIPrefab);
            
            RegistrationUI registrationUIPrefab = Resources.Load<RegistrationUI>("Canvas_Registration");
            RegistrationUI registrationUI = Instantiate(registrationUIPrefab);
            
            authorizationUI.SetRegistrationCanvas(registrationUI.gameObject);
            registrationUI.SetAuthorizationCanvas(authorizationUI.gameObject);
        }

    }
}