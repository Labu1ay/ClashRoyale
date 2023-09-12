﻿using UnityEngine;

[CreateAssetMenu(fileName = "_UsualMeleeAttack", menuName = "UnitState/UsualMeleeAttack")]
public class UsualMeleeAttack : UnitStateAttack {
    protected override bool TryFindTarget(out float stopAttackDistance) {
        Vector3 unitPosition = _unit.transform.position;

        bool hasEnemy = MapInfo.Instance.TryGetNearestWalkingUnit(unitPosition, _targetIsEnemy, out Unit enemy, out float distance);
        
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
}