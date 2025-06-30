using Fusion;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour, IPlayerLeft
{
    [Networked] public NetworkString<_16> PlayerName { get; set; }
    [Networked] public NetworkObject Obj { get; set; }
    [Networked] public PlayableChar CharIndex { get; set; }
    [Networked] public bool isSpawned { get; set; }
    
    public static PlayerInfo Local;
    
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public override void Spawned()
    {
        Object.Runner.SetPlayerObject(Object.InputAuthority, Object);
        if (false == Object.HasInputAuthority)
            return;
        Local = this;
        PlayFabClientAPI.GetPlayerProfile(
            new GetPlayerProfileRequest() { PlayFabId = GameManager.I.ID },
            result => RPC_SetInfo(result.PlayerProfile.DisplayName, PlayableChar.Aris),
            error => Debug.Log(error.Error));
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if(Object.Runner.IsShutdown)
            return;
        
        PlayerName = string.Empty;
    }
 
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_SetInfo(string name, PlayableChar charIndex)
    {
        PlayerName = name;
        gameObject.name = $"{name} Info";
        CharIndex = charIndex;
        isSpawned = true;
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (false == Object.HasStateAuthority) 
            return;

        if (player == Object.InputAuthority)
            Object.Runner.Despawn(Object);
    }
}
