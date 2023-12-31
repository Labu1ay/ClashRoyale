﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.Game {
    public class CardController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
        [SerializeField] private Image _image;
        [SerializeField] private float _dragSize = 0.5f;
        private bool _isDragging = false;
        private Vector3 _startPosition;
        private CardManager _cardManager;
        private int _index;

        public void Init(CardManager manager, int index, Sprite sprite) {
            _cardManager = manager;
            _index = index;
            SetSprite(sprite);
            _startPosition = transform.localPosition;
        }

        public void SetSprite(Sprite sprite) => _image.sprite = sprite;

        public void OnBeginDrag(PointerEventData eventData) {
            if (false) return; // esli mani ne hvataet
            _isDragging = true;

            transform.localScale = Vector3.one * _dragSize;
        }

        public void OnDrag(PointerEventData eventData) {
            if(_isDragging == false) return;
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData) {
            if(_isDragging == false) return;
            _isDragging = false;
            
            _cardManager.Release(_index, eventData.position);
            
            transform.localPosition = _startPosition;
            transform.localScale = Vector3.one;
        }
    }
}