﻿using UnityEngine;
using UnityEngine.AI;

public abstract class UnitStateNavMeshMove : UnitState {
    private NavMeshAgent _agent;
    protected bool _targetIsEnemy;

    protected Tower _nearestTower;
    public override void Constructor(Unit unit) {
        base.Constructor(unit);

        _targetIsEnemy = !_unit._isEnemy;
        _agent = _unit.GetComponent<NavMeshAgent>();
        if(_agent == null) Debug.LogError($"На персонаже {unit.name} нет компонента NavMeshAgent");
        
        _agent.speed = _unit.Parameters.Speed;
        _agent.radius = _unit.Parameters.ModelRadius;
        _agent.stoppingDistance = _unit.Parameters.StartAttackDistance;
    }
    

    public override void Init() {
        Vector3 unitPosition = _unit.transform.position;
        _nearestTower = MapInfo.Instance.GetNearestTower(in unitPosition, _targetIsEnemy);
        _agent.SetDestination(_nearestTower.transform.position);
    }

    public override void Run() {
        if(_nearestTower == null) Init();
        if (TryFindTarget(out UnitStateType changeType)) {
            _unit.SetState(changeType);
        }
            
    }

    public override void Finish() {
        _agent.SetDestination(_unit.transform.position);
    }

    protected abstract bool TryFindTarget(out UnitStateType changeType);
}