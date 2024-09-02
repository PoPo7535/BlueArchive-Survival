using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class Spawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    public List<GameObject> spawnPoints = new();
    public Transform playerPoint;
    public GameObject obj;
    private float spawnDelay = 0;
    public float spawnDelayMax = 2;

    // Update is called once per frame
    private bool _mouseButton0;

    private void OnEnable()
    {
        App.I.Runner.AddCallbacks(this);
    }

    private void OnDisable()
    {
        App.I.Runner.RemoveCallbacks(this);
    }

    void Update()
    {
        _mouseButton0 |= Input.GetMouseButton(0);
    }

    public override void FixedUpdateNetwork()
    {
        if (false == HasStateAuthority)
            return;
        
        spawnDelay += Runner.DeltaTime;
        if (spawnDelayMax < spawnDelay)
        {
            spawnDelay = 0;
            var randomIndex =  UnityEngine.Random.Range(0, spawnPoints.Count);
            Runner.Spawn(obj, spawnPoints[randomIndex].transform.position, Quaternion.identity);
        }
    }

    public struct NetworkInputData : INetworkInput
    {
        public const byte MOUSEBUTTON0 = 1;

        public NetworkButtons buttons;
        public Vector2 input;
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        data.buttons.Set( NetworkInputData.MOUSEBUTTON0, _mouseButton0);
        _mouseButton0 = false;
        
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        data.input = new Vector2(x, y);
        input.Set(data);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
         if (HasStateAuthority)
         {
             var obj = player == Runner.LocalPlayer ? GameManager.I.kayoko : GameManager.I.kayoko2;
             var playerObj = Runner.Spawn(
                 obj, 
                 playerPoint.position, 
                 Quaternion.identity,player);
             Runner.SetPlayerObject(player, playerObj);
         }
    }
    
    #region
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    #endregion

}
