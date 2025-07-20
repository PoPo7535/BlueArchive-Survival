using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Utility;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public partial class BattleSceneManager // Ready
{
    private readonly Dictionary<PlayerRef, bool> _select = new();
    public bool stopObjs = false;
    private bool CheckReady => _select.All(kvp => kvp.Value);
    [Networked] public TickTimer SelectTimer { set; get; }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_LevelUp()
    {
        selectPanel.Open(true);
        if (false == Object.HasStateAuthority)
            return;
        exp -= levelUpValue;
        SelectTimer = TickTimer.CreateFromSeconds(Runner, 10f);
    }
    public void ReadyInit()
    {
        foreach (var playerRef in App.I.GetAllPlayers())
            _select.Add(playerRef, false);
        // YZHJK7G22G
    }
    
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        _select.Remove(player);
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void Rpc_Ready(RpcInfo info = default)
    {
        _select[info.Source] = true;
        if (CheckReady)
        {
            selectPanel.Open(false);
            SelectTimer = TickTimer.None;
            foreach (var playerRef in App.I.GetAllPlayers())
                _select[playerRef] = false;
        }
    }
}
