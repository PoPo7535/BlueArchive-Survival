using System.Collections.Generic;
using System.Linq;
using Fusion;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [Networked] public NetworkString<_16> PlayerName { get; set; }
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
                RPC_SetInfo(result.PlayerProfile.DisplayName);
            },
            error =>
            {
                Debug.Log(error.Error);
            });

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_SetInfo(string str)
    {
        PlayerName = str;
        gameObject.name = $"{str} Info";
        LobbyRoomPanel.I.SetSlot();
    }
}
