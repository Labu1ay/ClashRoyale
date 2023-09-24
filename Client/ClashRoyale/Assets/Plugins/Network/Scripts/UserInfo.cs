using UnityEngine;

namespace Plugins.Network.Scripts {
    public class UserInfo : MonoBehaviour {
#region Singlton
        public static UserInfo Instance { get; private set; }

        private void Awake() {
            if (Instance) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
#endregion
        
        public int ID { get; private set; }

        public void SetID(int id) => ID = id;
    }
}