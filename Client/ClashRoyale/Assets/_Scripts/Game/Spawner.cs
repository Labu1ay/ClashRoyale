using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Game {
    public class Spawner : MonoBehaviour {
        public void Spawn(string id, in Vector3 spawnPoint, bool isEnemy) {
            Unit unitPrefab;
            Quaternion rotation = quaternion.identity;
            if (isEnemy) {
                unitPrefab = CardsInGame.Instance._enemyDeck[id].Unit;
                rotation = Quaternion.Euler(0, 180,0);
            }
            else {
                unitPrefab = CardsInGame.Instance._playerDeck[id].Unit;
            }
            
            Unit unit = Instantiate(unitPrefab, spawnPoint, rotation);
            unit.Init(isEnemy);
            MapInfo.Instance.AddUnit(unit);
        }
    }
}