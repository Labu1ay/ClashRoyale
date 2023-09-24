using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Network.Scripts {
    public class RegistrationUI : MonoBehaviour {
        [SerializeField] private InputField _login;
        [SerializeField] private InputField _password;
        [SerializeField] private InputField _confirmPassword;
        [SerializeField] private Button _apply;
        [SerializeField] private Button _signIn;
        [SerializeField] private GameObject _authorizationCanvas;
        [SerializeField] private GameObject _registrationCanvas;

        private NetworkBootstrap _networkBootstrap;

        private void Awake() {
            _networkBootstrap = NetworkBootstrap.Instance;
            _registrationCanvas = this.gameObject;
            
            _login.onEndEdit.AddListener(_networkBootstrap.Registration.SetLogin);
            _password.onEndEdit.AddListener(_networkBootstrap.Registration.SetPassword);
            _confirmPassword.onEndEdit.AddListener(_networkBootstrap.Registration.SetConfirmPassword);
            
            _apply.onClick.AddListener( () => {
                _apply.gameObject.SetActive(false);
                _signIn.gameObject.SetActive(false);
                _networkBootstrap.Registration.SignUp();
            });
            
            _signIn.onClick.AddListener(() => {
                _registrationCanvas.SetActive(false);
                _authorizationCanvas.SetActive(true);
            });

            _networkBootstrap.Registration.Error += () => {
                _apply.gameObject.SetActive(true);
                _signIn.gameObject.SetActive(true);
            };

            _networkBootstrap.Registration.Success += () => {
                _signIn.gameObject.SetActive(true);
            };
        }
        
        public void SetAuthorizationCanvas(GameObject canvas) => _authorizationCanvas = canvas;
    }
}