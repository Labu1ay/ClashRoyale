using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Network.Scripts {
    public class Authorization {
        private const string LOGIN = "login";
        private const string PASSWORD = "password";
        private string _login;
        private string _password;

        public event Action Error; 
        public event Action Success; 

        public void SetLogin(string login) {
            _login = login;
        }

        public void SetPassword(string password) {
            _password = password;
        }

        public void SignIn() {
            if (string.IsNullOrEmpty(_login) || string.IsNullOrEmpty(_password)) {
                ErrorMessage("Логин и/или пароль пустые");
                return;
            }

            string uri = URLLibrary.MAIN + URLLibrary.AUTHORIZATION;
            Dictionary<string, string> data = new Dictionary<string, string>() {
                { LOGIN, _login },
                { PASSWORD, _password }
            };
            NetworkBootstrap.Instance.Network.Post(uri, data, SuccessMessage, ErrorMessage);
            //Network.Instance.Post(uri, data, Success, ErrorMessage);
        }

        private void SuccessMessage(string data) {
            string[] result = data.Split('|');
            if (result.Length < 2 || result[0] != "ok") {
                ErrorMessage("Ответ с сервера пришел вот такой " + data);
                return;
            }

            if (int.TryParse(result[1], out int id)) {
                UserInfo.Instance.SetID(id);
                Debug.Log("Успешный вход. ID = " + id);
                Success?.Invoke();
            }
            else {
                ErrorMessage($"Не удалось расспарсить \"{result[1]}\" в INT. Полный ответ вот такой: {data}");
            }
        }

        private void ErrorMessage(string error) {
            Debug.LogError(error);
            Error?.Invoke();
        }
    }
}