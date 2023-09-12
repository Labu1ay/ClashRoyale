using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitStateNavMeshChase : UnitState {
    private NavMeshAgent _agent;
    protected bool _targetIsEnemy;
    protected Unit _targetUnit;
    protected float _startAttackDistance = 0f;

    public override void Constructor(Unit unit) {
        base.Constructor(unit);

        _targetIsEnemy = !_unit._isEnemy;
        _agent = _unit.GetComponent<NavMeshAgent>();
        if(_agent == null) Debug.LogError($"На персонаже {unit.name} нет компонента NavMeshAgent");
    }
    public override void Init() {
       FindTargetUnit(out _targetUnit);

       if (_agent == null || _targetUnit == null) {
           _unit.SetState(UnitStateType.Default);
           return;
       }
       _startAttackDistance = _unit.Parameters.StartAttackDistance + _targetUnit.Parameters.ModelRadius;
    }

    public override void Run() {
        if (_agent == null || _targetUnit == null) {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        float distanceToTarget = Vector3.Distance(_unit.transform.position, _targetUnit.transform.position);
        if (distanceToTarget > _unit.Parameters.StopChaseDistance) {_unit.SetState(UnitStateType.Default);}
        else if (distanceToTarget <= _startAttackDistance) _unit.SetState(UnitStateType.Attack);
        else _agent.SetDestination(_targetUnit.transform.position);
        
    }

    public override void Finish() {
        _agent.SetDestination(_unit.transform.position);
    }

#if UNITY_EDITOR
    public override void DebugDrawDistance(Unit unit) {
        Handles.color = Color.red;
        Handles.DrawWireDisc(unit.transform.position, Vector3.up, unit.Parameters.StartChaseDistance);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(unit.transform.position, Vector3.up, unit.Parameters.StopChaseDistance);
    }

#endif
    protected abstract void FindTargetUnit(out Unit targetUnit);
}