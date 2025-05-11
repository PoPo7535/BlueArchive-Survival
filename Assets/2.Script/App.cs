using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using WebSocketSharp;

public class App : SimulationBehaviour, INetworkRunnerCallbacks
{
    public static App I;
    [ReadOnly] public NetworkRunner runner;
    public bool IsHost => Runner.GetPlayerObject(Runner.LocalPlayer).HasStateAuthority;
    public PlayerInfo GetPlayerInfo(PlayerRef playerRef)
    {
        return Runner.GetPlayerObject(playerRef).GetComponent<PlayerInfo>();
    }


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
    public async Task ConnectingPlayer(PlayerRef player)
    {
        await UniTask.WaitUntil(() => null != Runner.GetPlayerObject(player));
        var info = Runner.GetPlayerObject(player).GetComponent<PlayerInfo>();
        await UniTask.WaitUntil(() => info.PlayerName != string.Empty && info.isSpawned);
    }

    public List<PlayerRef> GetAllPlayers()
    {
        var players = Runner.ActivePlayers.ToList();
        players.Sort((p1, p2) => p1.PlayerId.CompareTo(p2.PlayerId));
        return players;
    }
    
    public int GetPlayerIndex(PlayerRef playerRef)
    {
        var players = GetAllPlayers();
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].PlayerId == playerRef.PlayerId)
                return i;
        }

        return -1;
    }
    public void OnSessionListUpdated(NetworkRunner _, List<SessionInfo> sessionList) { }
    public void OnPlayerJoined(NetworkRunner _, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner _, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner _, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner _, NetworkObject obj, PlayerRef player) { }
    public void OnInput(NetworkRunner _, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner _, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner _, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner _) { }
    public void OnDisconnectedFromServer(NetworkRunner _, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner _, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner _, NetAddress remoteAddress, NetConnectFailedReason reason) {    }
    public void OnUserSimulationMessage(NetworkRunner _, SimulationMessagePtr message) {    }
    public void OnCustomAuthenticationResponse(NetworkRunner _, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner _, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner _, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner _, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner _) { }
    public void OnSceneLoadStart(NetworkRunner _) { }
}

