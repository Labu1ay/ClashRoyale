using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Network.Scripts {
    public class AuthorizationUI : MonoBehaviour {
        [SerializeField] private InputField _login;
        [SerializeField] private InputField _password;
        [SerializeField] private Button _signIn;
        [SerializeField] private Button _signUp;
        [SerializeField] private GameObject _authorizationCanvas;
        [SerializeField] private GameObject _registrationCanvas;

        private NetworkBootstrap _networkBootstrap;
        
        private void Awake() {
            _networkBootstrap = NetworkBootstrap.Instance;
            _authorizationCanvas = this.gameObject;
            
            _login.onEndEdit.AddListener(_networkBootstrap.Authorization.SetLogin);
            _password.onEndEdit.AddListener(_networkBootstrap.Authorization.SetPassword);
            
            _signIn.onClick.AddListener( () => {
                _signIn.gameObject.SetActive(false);
                _signUp.gameObject.SetActive(false);
                _networkBootstrap.Authorization.SignIn();
            });
            
            _signUp.onClick.AddListener(() => {
                _authorizationCanvas.SetActive(false);
                _registrationCanvas.SetActive(true);
            });

            _networkBootstrap.Authorization.Error += () => {
                _signIn.gameObject.SetActive(true);
                _signUp.gameObject.SetActive(true);
            };

        }

        public void SetRegistrationCanvas(GameObject canvas) => _registrationCanvas = canvas;
    }
}