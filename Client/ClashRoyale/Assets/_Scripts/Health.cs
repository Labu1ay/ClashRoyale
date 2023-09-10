using UnityEngine;
using UnityEngine.Serialization;


public class Health : MonoBehaviour {
    [field: SerializeField] public float Max { get; private set; } = 10f;
    [FormerlySerializedAs("_healthUIPrefab")] [SerializeField] private HealthBar healthBarPrefab;
    [SerializeField] private Transform _healthUIPosition;
    private HealthBar _healthBar;
    private float _current;

    private void Start() {
        _current = Max;
        if(healthBarPrefab)
            _healthBar = Instantiate(healthBarPrefab, _healthUIPosition.position, Quaternion.identity, transform);
    }

    public void ApplyDamage(float value) {
        _current -= value;
        
        if (_current < 0) {
            _current = 0;
            Destroy(gameObject);
        }
        
        Debug.Log($"Объект {name}:было {_current + value} , стало {_current}");
        if(_healthBar)
            _healthBar.UpdateHealth(_current, Max);
    }
    
}

interface IHealth {
    Health Health{ get; }
}