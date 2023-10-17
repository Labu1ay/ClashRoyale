using UnityEngine;


public class LocalSceneDependecy : MonoBehaviour {
    private void Start() {
#if UNITY_SERVER
        StartServer();
#else
        StartClient();
#endif
    }
    
#if UNITY_SERVER
    private void StartServer() {
        MatchmakingManager.Instance.AddNewSceneServer(this);
    }
#endif

    private void StartClient() {
        MatchmakingManager.Instance.AddNewSceneClient(this);
    }

    public void InitServer(int height) {
        
    }
}