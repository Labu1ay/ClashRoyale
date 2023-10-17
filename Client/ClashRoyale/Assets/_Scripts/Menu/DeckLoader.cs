using System;
using System.Collections.Generic;
using Plugins.Network.Scripts;
using UnityEngine;

namespace _Scripts.Menu {
    public class DeckLoader : MonoBehaviour {
        [SerializeField] private DeckManager _deckManager;
        
        [SerializeField] private List<int> _availableCards = new List<int>();
        [SerializeField] private int[] _selectedCards = new int[5];
        
        private void Start() {
            StartLoad();
        }

        private void StartLoad() {
            
            NetworkBootstrap.Instance.Network.Post(URLLibrary.MAIN + URLLibrary.GETDECKINFO,
                new Dictionary<string, string> { { "userID", UserInfo.Instance.ID.ToString() } },
                SuccessLoad, ErrorLoad);
        }

        private void ErrorLoad(string error) {
            Debug.LogError(error);
            StartLoad();
        }
        
        private void SuccessLoad(string data) {
            DeckData deckData = JsonUtility.FromJson<DeckData>(data);
            _selectedCards = new int[deckData.selectedIDs.Length];
            for (int i = 0; i < _selectedCards.Length; i++) {
                int.TryParse(deckData.selectedIDs[i], out _selectedCards[i]);
            }

            for (int i = 0; i < deckData.avaliableCards.Length; i++) {
                int.TryParse(deckData.avaliableCards[i].id, out int id);
                _availableCards.Add(id);
            }
            _deckManager.Init(_availableCards, _selectedCards);
        }
    }
    
    [Serializable]
    public class DeckData {
        public AvaliableCards[] avaliableCards;
        public string[] selectedIDs;
    }
    [Serializable]
    public class AvaliableCards {
        public string name;
        public string id;
    }
}