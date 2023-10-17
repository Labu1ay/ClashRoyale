using System.Collections.Generic;
using Plugins.Network.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Menu {
    public class RatingManager : MonoBehaviour {
        [SerializeField] private Text _ratingText;
        private void Start() {
            InitRating();
        }

        private void InitRating() {
            string uri = URLLibrary.MAIN + URLLibrary.GETRATING;
            Dictionary<string, string> data = new Dictionary<string, string>() {
                { "userID", UserInfo.Instance.ID.ToString() }
            };
            NetworkBootstrap.Instance.Network.Post(URLLibrary.MAIN + URLLibrary.GETRATING, data, Success, Error);
        }

        private void Success(string obj) {
            string[] result = obj.Split('|');
            if (result.Length != 3) {
                Error("Длинна массива != 3" + obj);
                return;
            }

            if (result[0] != "ok") {
                Error("Странный результат" + obj);
                return;
            }

            _ratingText.text = $"<color=green>{result[1]}</color> : <color=red>{result[2]}</color>"; 
        }

        private void Error(string obj) {
            Debug.LogError(obj);
        }
    }
}