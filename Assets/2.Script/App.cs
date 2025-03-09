using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class App : SimulationBehaviour, INetworkRunnerCallbacks
{
    public static App I;
    [ReadOnly] public NetworkRunner runner;
    public PlayerInfo localPlayerInfo => Object.Runner.GetPlayerObject(Object.Runner.LocalPlayer).GetComponent<PlayerInfo>();

    public enum Property
    {
        Password
    }
    
    private void Awake()
    {
        I = this;
        runner = GetComponent<NetworkRunner>();
        runner.AddCallbacks(this);
    }
    private void Start()
    {
        JoinSessionLobby();
    }

    private async void JoinSessionLobby()
    {
        PopUp.I.OpenPopUp("세션에 참가 중");
        var result = await runner.JoinSessionLobby(SessionLobby.ClientServer);
        PopUp.I.OpenPopUp(result);
    }

    public async void HostGame(GameMode gameMode, string sessionName, string password, Action okAction = null, Action errorAction =null) 
    {
        PopUp.I.OpenPopUp("호스트 중");
        // var loadSceneAsync  = SceneManager.LoadSceneAsync("2.Room");
        // await UniTask.WaitUntil(() => loadSceneAsync.isDone);
        var sessionProperties = new Dictionary<string, SessionProperty>();
        
        if (false == password.IsNullOrEmpty())
            sessionProperties.Add(Property.Password.ToString(), int.Parse(password));
        
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = sessionName,
            CustomLobbyName = "CustomLobbyName",
            IsOpen = true, IsVisible = true,
            PlayerCount = 3,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            ObjectProvider = gameObject.AddComponent<NetworkObjectProviderDefault>(),
            SessionProperties = sessionProperties,
            Scene = new SceneRef() { RawValue = 3 }
        });
        if (result.Ok)
            okAction?.Invoke();
        else
            errorAction?.Invoke();

        // var re= runner.LoadScene("2.Room").IsDone;
        
        PopUp.I.OpenPopUp(result);
    }
    public async void JoinGame(string sessionName) 
    {
        PopUp.I.OpenPopUp("조인 중");
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = sessionName,
            CustomLobbyName = "CustomLobbyName",
            IsOpen = true,
            IsVisible = true,
            PlayerCount = 3,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            ObjectProvider = gameObject.AddComponent<NetworkObjectProviderDefault>() ,
        });
        PopUp.I.OpenPopUp(result);
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}

