using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using Utility;

public class Spawner : LocalFusionSingleton<Spawner>, INetworkRunnerCallbacks
{
    public List<GameObject> spawnPoints = new();
    public Transform playerPoint;
    public GameObject obj;
    // private float _spawnDelay = 0;
    public float spawnDelayMax = 2;

    public override void FixedUpdateNetwork()
    {
        if (false == HasStateAuthority)
            return;

    }
    //
    // [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    // public void RPC_SkillSpawn(RpcInfo info = default)
    // {
    //     Runner.Spawn(GameManager.I.GetSkillScriptable(SkillType.Wheel).skillObj, Vector3.zero, Quaternion.identity, info.Source);
    // }
    
#if UNITY_EDITOR
    public void OnGUI()
    {
        if (false == HasStateAuthority)
            return;
        if (GUI.Button(new Rect(50,50,100,100), "Spawn"))
        {
            var randomIndex =  UnityEngine.Random.Range(0, spawnPoints.Count);
            Runner.Spawn(obj, spawnPoints[randomIndex].transform.position, Quaternion.identity);
        }
    }
#endif

    public override void Spawned()
    {
        if (false == Object.HasStateAuthority)
            return;
        var players = App.I.GetAllPlayers();
        foreach (var player in players)
        {
            var playerInfo = App.I.GetPlayerInfo(player);
            var baseObj = Runner.Spawn(
                GameManager.I.playerBase, 
                playerPoint.position, 
                Quaternion.identity,player);
            playerInfo.PlayerObject = baseObj.GetComponent<PlayerBase>();
        }
    }

    #region
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
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
