using UnityEngine;


public abstract class UnitStateAttack : UnitState {
    [SerializeField] protected float _damage = 1.5f;
    private float _delay = 1f;
    private float _time = 0f;
    private float _stopAttackDistance = 0f;
    protected bool _targetIsEnemy;
    protected Health _target;

    public override void Constructor(Unit unit) {
        base.Constructor(unit);
        _targetIsEnemy = !_unit._isEnemy;
        _delay = unit.Parameters.DamageDelay;
    }

    public override void Init() {
        if (TryFindTarget(out _stopAttackDistance) == false) {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        _time = 0f;
        _unit.transform.LookAt(_target.transform.position);
    }

    public override void Run() {
        if (_target == false) {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        _time += Time.deltaTime;
        if (_time < _delay) return;
        _time -= _delay;

        float distanceToTarget = Vector3.Distance(_unit.transform.position, _target.transform.position);
        if (distanceToTarget > _stopAttackDistance) {
            if (_target) _target.ApplyDamage(_damage);
        }

        Attack();
    }

    protected virtual void Attack() {
        _target.ApplyDamage(_damage);
    }

    public override void Finish() {
    }

    protected abstract bool TryFindTarget(out float stopAttackDistance);
}