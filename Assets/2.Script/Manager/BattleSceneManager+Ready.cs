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
    [Networked, Capacity(3), OnChangedRender(nameof(SelectChangedRender))] private NetworkDictionary<PlayerRef, bool> select => default;
    private bool CheckReady => select.All(kvp => kvp.Value);
    [Networked] public TickTimer SelectTimer { set; get; }

    public void Open()
    {
    }

    private void SelectChangedRender()
    {
        if (CheckReady)
        {
            SelectTimer = TickTimer.None;
            selectPanel.Open(false);
            ReSetSelect();
        }
    }
    private void LevelUp()
    {
        exp -= levelUpValue;
        SelectTimer = TickTimer.CreateFromSeconds(Runner, 10f);
        selectPanel.Open(true);
    }
    public void ReadyInit()
    {
        foreach (var playerRef in App.I.GetAllPlayers())
            select.Add(playerRef, false);
    }

    public void ReSetSelect()
    {
        foreach (var playerRef in App.I.GetAllPlayers())
            select.Set(playerRef, false);
    }
    
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        select.Remove(player);
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void Rpc_Ready(RpcInfo info = default)
    {
        select.Set(info.Source, true);
    }
}
