using System;
using _Scripts._Multiplayer;
using UnityEngine;

namespace _Scripts.Menu {
    public class MenuSubscriber : MonoBehaviour {
        [SerializeField] private DeckManager _deckManager;
        [SerializeField] private SelectedDeckUI _selectedDeckUI;
        [SerializeField] private SelectedDeckUI _selectedDeckUI2;
        [SerializeField] private AvailableDeckUI _availableDeckUI;

        private void Start() {
            _deckManager.UpdateSelected += _selectedDeckUI.UpdateCardsList;
            _deckManager.UpdateSelected += _selectedDeckUI2.UpdateCardsList;
            _deckManager.UpdateAvailable += _availableDeckUI.UpdateCardList;
        }

        private void OnDestroy() {
            _deckManager.UpdateSelected -= _selectedDeckUI.UpdateCardsList;
            _deckManager.UpdateSelected -= _selectedDeckUI2.UpdateCardsList;
            _deckManager.UpdateAvailable -= _availableDeckUI.UpdateCardList;
        }
    }
}