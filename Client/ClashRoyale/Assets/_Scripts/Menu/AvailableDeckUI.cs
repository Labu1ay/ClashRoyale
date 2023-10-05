using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Menu {
    public class AvailableDeckUI : MonoBehaviour {
        [SerializeField] private CardSelecter _selecter;
        [SerializeField] private List<AvailableCardUI> _availableCardUI = new List<AvailableCardUI>();
#region Editor
#if UNITY_EDITOR
        [SerializeField] private Transform _availableCardParent;
        [SerializeField] private AvailableCardUI _availableCardUIPrefab;


        public void SetAllCardsCount(Card[] cards) {
            for (int i = 0; i < _availableCardUI.Count; i++) {
                GameObject gameObject = _availableCardUI[i].gameObject;
                UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(gameObject);
            }

            _availableCardUI.Clear();


            for (int i = 1; i < cards.Length; i++) {
                AvailableCardUI card = Instantiate(_availableCardUIPrefab, _availableCardParent);
                card.Create(_selecter, cards[i], i);
                _availableCardUI.Add(card);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
#endregion

        public void UpdateCardList(IReadOnlyList<Card> available, IReadOnlyList<Card> selected) {
            for (int i = 0; i < _availableCardUI.Count; i++) {
                _availableCardUI[i].SetState(AvailableCardUI.CardStateType.Locked);
            }

            for (int i = 0; i < available.Count; i++) {
                _availableCardUI[available[i].ID-1].SetState(AvailableCardUI.CardStateType.Available);
            }

            for (int i = 0; i < selected.Count; i++) {
                _availableCardUI[selected[i].ID-1].SetState(AvailableCardUI.CardStateType.Selected);
            }
        }
    }
}