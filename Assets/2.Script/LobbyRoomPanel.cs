using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class LobbyRoomPanel : LocalFusionSingleton<LobbyRoomPanel>, ISetInspector, IPlayerJoined, IPlayerLeft
{
    [Serial, Read] private PlayerSlot playerSlot;
    [Serial, Read] private CharacterView characterView;

    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        GetComponent<CanvasGroup>();
        playerSlot = childs.First(tr => tr.name == "PlayerSlot").GetComponent<PlayerSlot>();
        characterView = FindObjectOfType<CharacterView>();
    }
    public async void PlayerJoined(PlayerRef player)
    {
        var obj = App.I.runner.Spawn(GameManager.I.playerInfo, Vector3.zero, Quaternion.identity, player);
        await App.I.ConnectingPlayer(player);
        playerSlot.SetPlayerSlot(player);
        
        var info = obj.GetComponent<PlayerInfo>();
        characterView.SetChar(player, info.CharIndex);
    }

    public void PlayerLeft(PlayerRef player)
    {
        playerSlot.SetAllPlayerSlot();
        characterView.SetAllChar(); 
    }

    public override void Spawned()
    {
        playerSlot.SetAllPlayerSlot();
        characterView.SetAllChar();
    }


    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SetChar(PlayableChar ch, RpcInfo info =default)
    {
        App.I.GetPlayerInfo(info.Source).CharIndex = ch;
        playerSlot.SetPlayerSlot(info.Source);
        if (ch == PlayableChar.None)
            characterView.Clear(info.Source);
        else
            characterView.SetChar(info.Source, ch);
    }
}
