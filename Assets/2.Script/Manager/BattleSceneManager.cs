using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public partial class BattleSceneManager : LocalFusionSingleton<BattleSceneManager>, ISetInspector, INetworkRunnerCallbacks
{
    [Networked, OnChangedRender(nameof(OnChangedExp))] public float exp { get; set; }
    public BattleData battleData = new();
    [Read, Serial] private AbilitySelectPanel _selectPanel;
    
    [Read, Serial] private ExpBar _expBar;
    [Read, Serial] private float _expUpdateTime = 0f;
    [Read, Serial] private float _levelUpValue = 100f;
    [Read, Serial] private const float ExpUpdateMaxTime = 1f;

    [Button, GUIColor(0,1f,0)]
    public void SetInspector()
    {
        _selectPanel = FindObjectOfType<AbilitySelectPanel>();
        _expBar = FindObjectOfType<ExpBar>();
    }

    public override void Spawned()
    {
        App.I.Runner.AddCallbacks(this);
        ReadyInit();
    }
    public override void FixedUpdateNetwork()
    {
        UpdateExp();
    }
    private void UpdateExp()
    {
        _expUpdateTime += Runner.DeltaTime;
        if (_expUpdateTime < ExpUpdateMaxTime)
            return;
        _expUpdateTime = 0f;
        if (0f < battleData.curExp)
        {
            RPC_AddExp(battleData.curExp);
            battleData.ReSetExp();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_AddExp(float addExp)
    {
        exp += addExp;
    }

    private void OnChangedExp()
    {
        var expValue = exp / _levelUpValue;
        _expBar.SetBar(expValue);
        if (1f <= expValue)
            LevelUp();
    }

    private void LevelUp()
    {
        exp -= _levelUpValue;
        _selectPanel.cg.ActiveCG(true);
    }

#region UnUsedCallback
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
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