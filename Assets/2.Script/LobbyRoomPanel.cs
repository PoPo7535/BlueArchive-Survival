using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class LobbyRoomPanel : FusionSingleton<LobbyRoomPanel>, ISetInspector, IPlayerJoined, IPlayerLeft
{
    [Serial, Read] private CanvasGroup cg;
    [Serial, Read] private Button cancelBtn;
    [Serial, Read] private PlayerSlot playerSlot;
    [Serial, Read] private CharacterView characterView;

    private readonly Dictionary<PlayerRef, bool> ready = new();
    private bool isReady;    
    
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        cg = GetComponent<CanvasGroup>();
        cancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
        playerSlot = childs.First(tr => tr.name == "PlayerSlot").GetComponent<PlayerSlot>();
        characterView = FindObjectOfType<CharacterView>();
    }
    private void Start()
    {
        cancelBtn.onClick.AddListener(async () =>
        {
            await App.I.runner.Shutdown(false);
            SceneManager.LoadScene("1.Lobby");
        });
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
        characterView.Clear(App.I.GetPlayerIndex(player));
    }

    public override void Spawned()
    {
        playerSlot.SetAllPlayerSlot();
        characterView.SetAllChar();
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_CharIndex(PlayableChar ch, RpcInfo info = default)

    {
        App.I.GetPlayerInfo(info.Source).CharIndex = ch;
        playerSlot.SetPlayerSlot(info.Source);
        characterView.SetChar(info.Source, ch);
    }
}
