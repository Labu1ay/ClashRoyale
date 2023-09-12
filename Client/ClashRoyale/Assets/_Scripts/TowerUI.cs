﻿using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour {
    [FormerlySerializedAs("_unit")] [SerializeField] private Tower _tower;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private Image _fillHealthImage;
    private float _maxHealth;

    private void Start() {
        _healthBar.SetActive(false);
        _maxHealth = _tower.Health.Max;

        _tower.Health.UpdateHealth += UpdateHealth;
    }

    private void UpdateHealth(float currentValue) {
        _healthBar.SetActive(true);
        _fillHealthImage.fillAmount = currentValue / _maxHealth;
    }

    private void OnDestroy() {
        _tower.Health.UpdateHealth -= UpdateHealth;
    }
}