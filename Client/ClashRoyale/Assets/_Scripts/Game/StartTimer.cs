using UnityEngine;
using UnityEngine.UI;
using static _Scripts.Game.TimerManager;

namespace _Scripts.Game {
    public class StartTimer : MonoBehaviour {
        [SerializeField] private GameObject _destroyedObject;
        [SerializeField] private Text _text;
        
        private void StartTick(string jsonTick) {
            Tick tick = JsonUtility.FromJson<Tick>(jsonTick);
            if (tick.tick < 10) {
                _text.text =(10 - tick.tick).ToString();
            }
            else {
                Destroy(_destroyedObject);
            }
            
        }
    }
}