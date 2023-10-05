using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Menu {
    public class AvailableCardUI : MonoBehaviour {
        [SerializeField] private Text _text;
        [SerializeField] private Color _availableColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _lockedColor;
        private CardStateType _currentState = CardStateType.None;
        [SerializeField] private CardSelecter _selecter;
        [SerializeField] private int _id;
#region Editor
#if UNITY_EDITOR
        [SerializeField] private Image _image;

        public void Create(CardSelecter selecter, Card card, int id) {
            _selecter = selecter;
            _id = id;
            _image.sprite = card.Sprite;
            _text.text = card.Name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
#endregion
        public enum CardStateType {
            None = 0,
            Available = 1,
            Selected = 2,
            Locked = 3
            
        }

        public void SetState(CardStateType stateType) {
            _currentState = stateType;
            switch (stateType) {
                case CardStateType.Available :
                    _text.color = _availableColor;
                    break;
                case CardStateType.Selected :
                    _text.color = _selectedColor;
                    break;
                case CardStateType.Locked :
                    _text.color = _lockedColor;
                    break;
                default:
                    break;
            }
        }
        public void Click() {
            switch (_currentState) {
                case CardStateType.Available :
                    _selecter.SelectCard(_id);
                    SetState(CardStateType.Selected);
                    break;
                case CardStateType.Selected :
                    break;
                case CardStateType.Locked :
                    break;
                default:
                    break;
            }
        }
    }
}