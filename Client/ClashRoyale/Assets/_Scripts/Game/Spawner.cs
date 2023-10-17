using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Game {
    public class Spawner : MonoBehaviour {
        [System.Serializable]
        public class SpawnData {
            public string cardID;
            public float x;
            public float y;
            public float z;
            public uint serverTime;
            
            public SpawnData(string cardID, float x, float y, float z) {
                this.cardID = cardID;
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        [SerializeField] private TimerManager _timerManager;
        private Queue<GameObject> _galagrams = new Queue<GameObject>();

        private void Start() {
            MultiplayerManager.Instance.SpawnPlayer += SpawnPlayer;
            MultiplayerManager.Instance.SpawnEnemy += SpawnEnemy;
            MultiplayerManager.Instance.Cheat += CancelSpawn;
        }

        private void SpawnPlayer(string json) => StartCoroutine(Spawn(json, false));
        private void SpawnEnemy(string json) => StartCoroutine(Spawn(json, true));
        
        private void OnDestroy() {
            MultiplayerManager.Instance.SpawnPlayer -= SpawnPlayer;
            MultiplayerManager.Instance.SpawnEnemy -= SpawnEnemy;
            MultiplayerManager.Instance.Cheat -= CancelSpawn;
        }

        private IEnumerator Spawn(string jsonSpawnData, bool isEnemy) {
            SpawnData data = JsonUtility.FromJson<SpawnData>(jsonSpawnData);
            string id = data.cardID;
            Vector3 spawnPoint = new Vector3(data.x, data.y, data.z);
            
            Unit unitPrefab;
            Quaternion rotation = quaternion.identity;
            if (isEnemy) {
                unitPrefab = CardsInGame.Instance._enemyDeck[id].Unit;
                rotation = Quaternion.Euler(0, 180,0);
                spawnPoint *= -1;
            }
            else {
                unitPrefab = CardsInGame.Instance._playerDeck[id].Unit;
            }

            float delay = _timerManager.GetConvertTime(data.serverTime) - Time.time;

            if (delay < 0) {
                Debug.LogError("Всё пошло не по плану");
            }
            else {
                yield return new WaitForSeconds(delay);
            }
            
            if (isEnemy == false && _galagrams.Count > 0) {
                GameObject galagram = _galagrams.Dequeue();
                Destroy(galagram);
            }
            
            Unit unit = Instantiate(unitPrefab, spawnPoint, rotation);
            unit.Init(isEnemy);
            MapInfo.Instance.AddUnit(unit);
        }

        private void CancelSpawn() {
            if (_galagrams.Count < 1) return;
            
            GameObject galagram = _galagrams.Dequeue();
            Destroy(galagram);
            
        }

        public void SendSpawn(string id, in Vector3 spawnPoint) {
            GameObject galagram = Instantiate(CardsInGame.Instance._playerDeck[id].Galagram, spawnPoint, quaternion.identity);
            _galagrams.Enqueue(galagram);


            Dictionary<string, string> data = new Dictionary<string, string>() {
                { "json", JsonUtility.ToJson(new SpawnData(id, spawnPoint.x, spawnPoint.y, spawnPoint.z)) }
            };
            MultiplayerManager.Instance.SendMessage("spawn", data);
        }
    }
}