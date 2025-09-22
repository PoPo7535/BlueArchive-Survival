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
    public bool PauseObject => false == SelectTimer.ExpiredOrNotRunning(Runner);
    private bool CheckReady => _select.All(kvp => kvp.Value);
    [Networked] public TickTimer SelectTimer { set; get; }
    [Read] public float selectTime = 10f;
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_LevelUp(bool active)
    {
        PanelOpen(active);
    }
    public void ReadyInit()
    {
        foreach (var playerRef in App.I.GetAllPlayers())
            _select.Add(playerRef, false);
    }
    
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        _select.Remove(player);
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void Rpc_SelectReady(RpcInfo info = default)
    {
        _select[info.Source] = true;
        if (false == CheckReady)
            return;
        foreach (var playerRef in App.I.GetAllPlayers())
            _select[playerRef] = false;
        if (exp < levelUpValue)
            PanelOpen(false);
        else
            PanelOpen(true);
        // RPC_LevelUp();
    }

    private void PanelOpen(bool open)
    {
        selectPanel.Open(open);
        if (false == HasStateAuthority)
            return;
        SelectTimer = open ? 
            TickTimer.CreateFromSeconds(Runner, selectTime) : 
            TickTimer.None;
    }
}
