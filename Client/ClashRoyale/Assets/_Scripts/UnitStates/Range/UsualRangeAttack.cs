﻿using UnityEngine;

[CreateAssetMenu(fileName = "_UsualRangeAttack", menuName = "UnitState/UsualRangeAttack")]
public class UsualRangeAttack : UnitStateAttack {
    [SerializeField] private Arrow _arrow;

    protected override bool TryFindTarget(out float stopAttackDistance) {
        Vector3 unitPosition = _unit.transform.position;

        bool hasEnemy = MapInfo.Instance.TryGetNearestAnyUnit(unitPosition, _targetIsEnemy, out Unit enemy, out float distance);
        
        if (hasEnemy && distance - enemy.Parameters.ModelRadius <= _unit.Parameters._startAttackDistance + _unit.Parameters.ModelRadius) {
            _target = enemy.Health;
            stopAttackDistance = _unit.Parameters._stopAttackDistance + enemy.Parameters.ModelRadius;
            return true;
        }

        Tower targetTower = MapInfo.Instance.GetNearestTower(unitPosition, _targetIsEnemy);
        if (targetTower.GetDistance(unitPosition) <= _unit.Parameters._startAttackDistance) {
            _target = targetTower.Health;
            stopAttackDistance = _unit.Parameters._stopAttackDistance + targetTower.Radius;
            return true;
        }

        stopAttackDistance = 0f;
        return false;
    }

    protected override void Attack() {
        Vector3 unitPosition = _unit.transform.position;
        Vector3 targetPosition = _target.transform.position;
        
        Arrow arrow = Instantiate(_arrow, unitPosition, Quaternion.identity);
        arrow.Init(targetPosition);
        float delay = Vector3.Distance(unitPosition, targetPosition) / arrow.Speed;
        _target.ApplyDelayDamage(delay, _damage);
    }
}