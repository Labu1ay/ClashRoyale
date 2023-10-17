using System;
using System.Collections.Generic;
using Plugins.Network.Scripts;
using UnityEngine;
using System.Linq;

namespace _Scripts.Menu {
    public class DeckManager : MonoBehaviour {
        [SerializeField] private GameObject _lockScreenCanvas;
        
        [field: SerializeField] public CardsLibrary library { get; private set; }
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
        [field: SerializeField] public AvailableDeckUI _availableDeckUI { get; private set; }
#endif
#endregion

        public void Init(List<int> availablesCardIndexes, int[] selectedCardIndexes) {
            for (int i = 0; i < availablesCardIndexes.Count; i++)
                _availableCards.Add(library.cards[availablesCardIndexes[i]]);

            for (int i = 0; i < selectedCardIndexes.Length; i++)
                _selectedCards.Add(library.cards[selectedCardIndexes[i]]);

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
            NetworkBootstrap.Instance.Network.Post(uri, data, (s) => SendSuccess(s, success), Error);
            
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

        [System.Serializable]
        private class Wrapper {
            public int[] IDs;

            public Wrapper(int[] ids) {
                IDs = ids;
            }

        }
    }
}