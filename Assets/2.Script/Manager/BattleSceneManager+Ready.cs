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


// Ready
public partial class BattleSceneManager
{
    private readonly Dictionary<PlayerRef, bool> select = new();
    private bool CheckReady() => select.All(kvp => kvp.Value);
    [Networked] private TickTimer TickTimer { set; get; }

    public void Open()
    {
        TickTimer = TickTimer.CreateFromSeconds(Runner, 10f);
    }
    public void ReadyInit()
    {
        foreach (var playerRef in App.I.GetAllPlayers())
            select.Add(playerRef, false);
    }
    public void PlayerLeft(PlayerRef player)
    {
        select.Remove(player);
    }

    public void ReSetSelect()
    {
        foreach (var playerRef in App.I.GetAllPlayers())
            select[playerRef] = false;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority,HostMode =RpcHostMode.SourceIsHostPlayer)]
    public void Rpc_Ready(RpcInfo info = default)
    {
        select[info.Source] = true;
    }
}
