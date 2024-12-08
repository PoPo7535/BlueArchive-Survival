using System;
using Fusion;
using PlayFab;
using PlayFab.AuthenticationModels;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnChangedName))] public NetworkString<_16> PlayerName { get; set; }
    [Networked] public int CharIndex { get; set; }


    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnChangedName()
    {
        gameObject.name = $"{PlayerName} Info";
    }
    public override void Spawned()
    {
        if (false == Object.HasInputAuthority)
            return;
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest() 
                { PlayFabId = GameManager.I.ID },
            result =>
            {
                Debug.Log(result.PlayerProfile.DisplayName);
                RPC_SetName(result.PlayerProfile.DisplayName);

            },
            error =>
            {
                Debug.Log(error.Error);
            });

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)]
    private void RPC_SetName(string str)
    {
        Debug.Log("2222222222222222222222222222222222222222222222222222222");
        PlayerName = str;
        LobbyRoomPanel.I.playerSlot.SetSlot(1, PlayerName.Value);
    }
}
