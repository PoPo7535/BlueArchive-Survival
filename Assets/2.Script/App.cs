using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class App : SimulationBehaviour, INetworkRunnerCallbacks
{
    // Start is called before the first frame update
    public static App I;
    [Fusion.ReadOnly] public NetworkRunner runner;
    // public PlayerInfo playerInfo;

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

    public async void HostGame(GameMode gameMode, int password, Action okAction = null, Action errorAction =null) 
    {
        PopUp.I.OpenPopUp("호스트 중");
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = "SessionName",
            CustomLobbyName = "CustomLobbyName",
            IsOpen = true,
            IsVisible = true,
            PlayerCount = 3,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            ObjectProvider = gameObject.AddComponent<NetworkObjectProviderDefault>() ,
            SessionProperties = new Dictionary<string, SessionProperty>()
            {
                {"Password",password}
            }
            
        });
        if(result.Ok)
            okAction?.Invoke();
        else
            errorAction?.Invoke();

        PopUp.I.OpenPopUp(result);
    }
    public async void ClientGame(string sessionName) 
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
            SessionProperties = new Dictionary<string, SessionProperty>()
            {
                {"Password",int.MaxValue}
            }
            
        });
        PopUp.I.OpenPopUp(result);
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        foreach (var info in sessionList)
        {
            foreach (var Properties in info.Properties)
            {
                Debug.Log(Properties.Key);
            }
        }
    }
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

