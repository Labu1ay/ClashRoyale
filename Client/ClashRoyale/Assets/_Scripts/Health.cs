using UnityEngine;


public class Health : MonoBehaviour {
    [field: SerializeField] public float Max { get; private set; } = 10f;
    private float _current;

    private void Start() {
        _current = Max;
    }

    public void ApplyDamage(float value) {
        _current -= value;
        if (_current < 0) _current = 0;
        
        Debug.Log($"Объект {name}:было {_current + value} , стало {_current}");
    }
    
}

interface IHealth {
    Health Health{ get; }
}