﻿using System;
using System.Collections.Generic;
using UnityEngine;


public class MapInfo : MonoBehaviour {
#region SingletonOneScene
    public static MapInfo Instance { get; private set; }

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }
#endregion
    
    [field: SerializeField] public List<Tower> _enemyTowers { get; private set; } = new List<Tower>();
    [field: SerializeField] public List<Tower> _playerTowers { get; private set; } = new List<Tower>();
    
    [field: SerializeField] public List<Unit> _enemyUnits { get; private set; } = new List<Unit>();
    [field: SerializeField] public List<Unit> _playerUnits { get; private set; } = new List<Unit>();

    public void AddToList<T>(List<T> list, T obj) => list.Add(obj);
    public void RemoveFromList<T>(List<T> list, T obj) => list.Remove(obj);

    public bool TryGetNearestUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance) {
        List<Unit> units = enemy ? _enemyUnits : _playerUnits;
        unit = GetNearest<Unit>(currentPosition, units, out distance); 
        return unit;
    }

    public Tower GetNearestTower(in Vector3 currentPosition, bool enemy) {
        List<Tower> towers = enemy ? _enemyTowers : _playerTowers;
        return GetNearest<Tower>(in currentPosition, towers, out float distance);
    }

    private T GetNearest<T>(in Vector3 currentPosition, List<T> objects, out float distance) where T : MonoBehaviour {
        distance = float.MaxValue;
        if (objects.Count <= 0) return null;

        distance = Vector3.Distance(currentPosition, objects[0].transform.position);
        T nearest = objects[0];

        for (int i = 1; i < objects.Count; i++) {
            float tempDistance = Vector3.Distance(currentPosition, objects[i].transform.position);
            
            if (tempDistance > distance) continue;

            nearest = objects[i];
            distance = tempDistance;
        }

        return nearest;
    }
        
    
}