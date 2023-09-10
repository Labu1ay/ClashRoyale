using UnityEngine;

[RequireComponent(typeof(Health))]
public class Tower : MonoBehaviour, IHealth {
    [field: SerializeField] public bool _isEnemyTower { get; private set; } = false;
    [field: SerializeField] public Health Health { get; private set; }
    
    [field: SerializeField] public float Radius { get; private set; } = 2f;

    public float GetDistance(in Vector3 point) => Vector3.Distance(transform.position, point) - Radius;

    private MapInfo _info;
    private void Start() {
        _info = MapInfo.Instance;
        if(!_isEnemyTower) _info.AddToList(_info._playerTowers, this);
        else _info.AddToList(_info._enemyTowers, this);
    }

    private void OnDestroy() {
        if(!_isEnemyTower) _info.RemoveFromList(_info._playerTowers, this);
        else _info.RemoveFromList(_info._enemyTowers, this);
    }
}