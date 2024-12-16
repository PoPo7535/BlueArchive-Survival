using System.Collections.Generic;
using System.Linq;
using Fusion;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour, IPlayerLeft
{
    [Networked] public NetworkString<_16> PlayerName { get; set; }
    [Networked] public int CharIndex { get; set; }


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
        
        LobbyRoomPanel.I.SetSlot();
    }
 
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_SetInfo(string name)
    {
        PlayerName = name;
        gameObject.name = $"{name} Info";
        LobbyRoomPanel.I.SetSlot();
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (false == Object.HasStateAuthority) 
            return;

        if (player == Object.InputAuthority)
        {
            Object.Runner.Despawn(Object);
        }
    }
}
