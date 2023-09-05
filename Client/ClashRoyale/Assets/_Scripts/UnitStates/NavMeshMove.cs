using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "_NavMeshMove", menuName = "UnitState/NavMeshMove")]
public class NavMeshMove : UnitState {
     
    private NavMeshAgent _agent;
    private Vector3 _targetPosition;
    private bool _targetIsEnemy;

    private Tower _nearestTower;

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
        _targetPosition = _nearestTower.transform.position;
        
        _agent.SetDestination(_targetPosition);
    }

    public override void Run() {
        if(TryAttackTower()) return;
        if(TryAttackUnit()) return;
    }

    private bool TryAttackTower() {
        float distanceToTarget = _nearestTower.GetDistance(_unit.transform.position);
        if (distanceToTarget <= _unit.Parameters.StartAttackDistance) {
            _unit.SetState(UnitStateType.Attack);
            return true;
        }
        return false;
    }

    private bool TryAttackUnit() {
        bool hasEnemy = MapInfo.Instance.TryGetNearestUnit(_unit.transform.position, out Unit enemy, _targetIsEnemy, out float distance);

        if (hasEnemy == false) return false;
        if (_unit.Parameters.StartChaseDistance >= distance + enemy.Parameters.ModelRadius) {
            _unit.SetState(UnitStateType.Chase);
            return true;
        }
        return false;
    }

    public override void Finish() {
        _agent.isStopped = true;
    }
}
