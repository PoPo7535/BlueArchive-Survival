using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class App : SimulationBehaviour, INetworkRunnerCallbacks
{
    // Start is called before the first frame update
    public NetworkRunner runner;
    public static App I;


    private void Start()
    {
        I = this;
        runner = GetComponent<NetworkRunner>();
        runner.AddCallbacks(this);
        JoinSessionLobby();
    }

    private async void JoinSessionLobby()
    {
        var result = await runner.JoinSessionLobby(SessionLobby.ClientServer);

    }
    public async void HostGame(GameMode gameMode, int password) 
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

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log(player);
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log(sessionList.Count);
        foreach (var info in sessionList)
        {
            foreach (var Properties in info.Properties)
            {
                Debug.Log(Properties.Key);
            }
        }
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log(nameof(OnConnectedToServer));
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log(nameof(OnDisconnectedFromServer));
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log(nameof(OnConnectRequest));
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log(nameof(OnConnectFailed));
    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log(nameof(OnUserSimulationMessage));
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log(nameof(OnCustomAuthenticationResponse));
    }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log(nameof(OnHostMigration));
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        Debug.Log(nameof(OnReliableDataReceived));
    }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log(nameof(OnReliableDataProgress));
    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log(nameof(OnSceneLoadDone));
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log(nameof(OnSceneLoadStart));
    }
}

