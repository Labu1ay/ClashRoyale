using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Menu {
    public class CardSelecter : MonoBehaviour {
        [SerializeField] private DeckManager _deckManager;
        [SerializeField] private AvailableDeckUI _availableDeckUI;
        [SerializeField] private SelectedDeckUI _selectedDeckUI;
        private List<Card> _availableCards = new List<Card>();
        private List<Card> _selectedCards = new List<Card>();
        private int _selectToggleIndex = 0;

        public IReadOnlyList<Card> AvailableCards { get { return _availableCards; } }

        public IReadOnlyList<Card> SelectedCards { get { return _selectedCards; } }

        private void OnEnable() {
            FillListFromManager();
        }

        private void FillListFromManager() {
            _availableCards.Clear();
            for (int i = 0; i < _deckManager.AvailableCards.Count; i++) {
                _availableCards.Add(_deckManager.AvailableCards[i]);
            }

            _selectedCards.Clear();
            for (int i = 0; i < _deckManager.SelectedCards.Count; i++) {
                _selectedCards.Add(_deckManager.SelectedCards[i]);
            }
        }

        public void SetSelectToggleIndex(int index) {
            _selectToggleIndex = index;
        }

        public void SelectCard(int cardId) {
            _selectedCards[_selectToggleIndex] = _availableCards[cardId-1];
            
            _selectedDeckUI.UpdateCardsList(SelectedCards);
            _availableDeckUI.UpdateCardList(AvailableCards, SelectedCards);
        }

        public void SaveChanges() {
            _deckManager.ChangesDeck(SelectedCards, CloseChangesWindow);
        }
        
        public void CancelChanged() {
            FillListFromManager();
            
            _selectedDeckUI.UpdateCardsList(SelectedCards);
            _availableDeckUI.UpdateCardList(AvailableCards, SelectedCards);
            CloseChangesWindow();
        }
        [Space(24), Header("Логика переключения канвасов")]
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _cardSelectCanvas;
        
        public void CloseChangesWindow() {
            _cardSelectCanvas.SetActive(false);
            _mainCanvas.SetActive(true);
        }
    }
}