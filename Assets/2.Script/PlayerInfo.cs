using System;
using Fusion;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour, IPlayerLeft
{
    [Networked] public NetworkString<_16> PlayerName { get; set; }
    [Networked] public int CharIndex { get; set; } = -1;


    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void Spawned()
    {
        Object.Runner.SetPlayerObject(Object.InputAuthority, Object);

        if (false == Object.HasInputAuthority)
            return;

        PlayFabClientAPI.GetPlayerProfile(
            new GetPlayerProfileRequest() { PlayFabId = GameManager.I.ID },
            result => RPC_SetInfo(result.PlayerProfile.DisplayName),
            error => Debug.Log(error.Error));

    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if(Object.Runner.IsShutdown)
            return;
        
        PlayerName = string.Empty;
    }
 
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_SetInfo(string name)
    {
        PlayerName = name;
        gameObject.name = $"{name} Info";
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (false == Object.HasStateAuthority) 
            return;

        if (player == Object.InputAuthority)
            Object.Runner.Despawn(Object);
    }

    public void OnGUI()
    {
        if (false == Object.HasInputAuthority)
            return;
        if (GUI.Button(new Rect(50, 50, 150, 100), "카요코"))
        {
            RPC_CharIndex(1);
        }
        
        if (GUI.Button(new Rect(50, 150, 150, 100), "카요코2"))
        {
            RPC_CharIndex(2);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_CharIndex(int index) => CharIndex = index;
}
