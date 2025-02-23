using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerSlot : MonoBehaviour, ISetInspector
{
    [Serial, Read] private PlayerSlotItem[] playerSlots;
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        playerSlots = new PlayerSlotItem[3];
        playerSlots[1] = childs.First(tr => tr.name == "PlayerSlot (1)").GetComponent<PlayerSlotItem>();
        playerSlots[0] = childs.First(tr => tr.name == "PlayerSlot (2)").GetComponent<PlayerSlotItem>();
        playerSlots[2] = childs.First(tr => tr.name == "PlayerSlot (3)").GetComponent<PlayerSlotItem>();
    }

    public void Start()
    {
        foreach (var player in App.I.runner.ActivePlayers)
        {
            App.I.runner.GetPlayerObject(player);
        }
    }

    public async void SetAllPlayerSlot()
    {
        await ConnectingPlayer(App.I.Runner.LocalPlayer);
        var players = App.I.Runner.ActivePlayers.ToList();
        players.Sort((p1, p2) => p1.PlayerId.CompareTo(p2.PlayerId));
        for (int i = 0; i < players.Count; ++i)
        {
            var playerInfo = App.I.Runner.GetPlayerObject(players[i]).GetComponent<PlayerInfo>();
            playerSlots[i].SetSlot(playerInfo.PlayerName.Value);
        }
        
        for (int i = players.Count; i < 3; ++i)
            playerSlots[i].ClearSlot();
    }

    public async void SetPlayerSlot(PlayerRef playerRef)
    {
        await ConnectingPlayer(playerRef);
        var players = App.I.Runner.ActivePlayers.ToList();
        players.Sort((p1, p2) => p1.PlayerId.CompareTo(p2.PlayerId));

        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i] == playerRef)
            {
                var playerInfo = App.I.Runner.GetPlayerObject(playerRef).GetComponent<PlayerInfo>();
                playerSlots[i].SetSlot(playerInfo.PlayerName.Value);
                break;
            }
        }
    }
    private async Task ConnectingPlayer(PlayerRef player)
    {
        await UniTask.WaitUntil(() => null != App.I.Runner.GetPlayerObject(player));
        var info = App.I.Runner.GetPlayerObject(player).GetComponent<PlayerInfo>();
        await UniTask.WaitUntil(() => info.PlayerName != string.Empty);
    }

}
