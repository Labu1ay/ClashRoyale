using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Colyseus;
using Plugins.Network.Scripts;

public class MultiplayerManager : ColyseusManager<MultiplayerManager> {
    private const string RoomName = "state_handler";
    private const string GetReadyName = "GetReady";
    private const string StartGameName = "Start";
    private const string CancelStartName = "CancelStart";
    
    private ColyseusRoom<State> _room;

    public event Action GetReady;
    public event Action<string> StartGame;
    public event Action CancelStart;
    
    public string clientID {
        get {
            if (_room == null) return "";
            else return _room.SessionId;
        }
    }

    protected override void Awake() {
        base.Awake();
        Instance.InitializeClient();
        DontDestroyOnLoad(gameObject);
    }

    public async Task Connect() {
        _room = await Instance.client.JoinOrCreate<State>(RoomName, new Dictionary<string, object>(){ {"id", UserInfo.Instance.ID} });
        _room.OnMessage<object>(GetReadyName, (empty) => GetReady?.Invoke() );
        _room.OnMessage<string>(StartGameName, (jsonDecks) => StartGame?.Invoke(jsonDecks) );
        _room.OnMessage<object>(CancelStartName, (empty) => CancelStart?.Invoke() );
    }

    public void Leave() {
        _room?.Leave();
        _room = null;
    }
    
}