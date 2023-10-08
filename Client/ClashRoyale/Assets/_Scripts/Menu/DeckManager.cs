using System;
using System.Collections.Generic;
using Plugins.Network.Scripts;
using TMPro;
using UnityEngine;
using Network = Plugins.Network.Scripts.Network;
using System.Linq;

namespace _Scripts.Menu {
    public class DeckManager : MonoBehaviour {
        [SerializeField] private GameObject _lockScreenCanvas;
        
        [SerializeField] private Card[] _cards;
        [SerializeField] private List<Card> _availableCards = new List<Card>();
        [SerializeField] private List<Card> _selectedCards = new List<Card>();

        public IReadOnlyList<Card> AvailableCards {
            get { return _availableCards; }
        }

        public IReadOnlyList<Card> SelectedCards {
            get { return _selectedCards; }
        }

        public event Action<IReadOnlyList<Card>, IReadOnlyList<Card>> UpdateAvailable;
        public event Action<IReadOnlyList<Card>> UpdateSelected;

#region Editor
#if UNITY_EDITOR
        [SerializeField] private AvailableDeckUI _availableDeckUI;
        private void OnValidate() {
            _availableDeckUI.SetAllCardsCount(_cards);
        }
#endif
#endregion

        public void Init(List<int> availablesCardIndexes, int[] selectedCardIndexes) {
            for (int i = 0; i < availablesCardIndexes.Count; i++)
                _availableCards.Add(_cards[availablesCardIndexes[i]]);

            for (int i = 0; i < selectedCardIndexes.Length; i++)
                _selectedCards.Add(_cards[selectedCardIndexes[i]]);

            UpdateAvailable?.Invoke(AvailableCards, SelectedCards);
            UpdateSelected?.Invoke(SelectedCards);
            
            _lockScreenCanvas.SetActive(false);
        }

        public void ChangesDeck(IReadOnlyList<Card> selectedCards, Action success) {
            _lockScreenCanvas.SetActive(true);
            int[] IDs = new int[selectedCards.Count];
            for (int i = 0; i < IDs.Length; i++) {
                IDs[i] = selectedCards[i].ID;
            }

            string json = JsonUtility.ToJson(new Wrapper(IDs));
            string uri = URLLibrary.MAIN + URLLibrary.SETSELECTDECK;
            Dictionary<string, string> data = new Dictionary<string, string> {
                { "userID", UserInfo.Instance.ID.ToString() },
                { "json", json }
            };
            success += () => {
                for (int i = 0; i < _selectedCards.Count; i++) {
                    _selectedCards[i] = selectedCards[i];
                }

                UpdateSelected?.Invoke(SelectedCards);
            };
            new Network().Post(uri, data, (s) => SendSuccess(s, success), Error);
            
        }

        private void SendSuccess(string obj, Action success) {
            if (obj != "ok") {
                Error(obj);
                return;
            }

            success?.Invoke();
            _lockScreenCanvas.SetActive(false);
        }

        private void Error(string obj) {
            Debug.LogError("Неудачная попытка отправки новой колоды: " + obj);
            _lockScreenCanvas.SetActive(false);
        }

        public bool TryGetDeck(string[] cardsIDs, out Dictionary<string, Card> deck) {
            deck = new Dictionary<string, Card>();
            for (int i = 0; i < cardsIDs.Length; i++) {
                if (int.TryParse(cardsIDs[i], out int id) == false || id == 0) return false;
                Card card = _cards.FirstOrDefault(c => c.ID == id);
                if (card == null) return false;
                
                deck.Add(cardsIDs[i], card);
            }

            return true;
        }

        [System.Serializable]
        private class Wrapper {
            public int[] IDs;

            public Wrapper(int[] ids) {
                IDs = ids;
            }

        }
    }

    [Serializable]
    public class Card {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Unit Unit { get; private set; }
        
    }
}