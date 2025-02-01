using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerSlot : MonoBehaviour, ISetInspector
{
    [Serial, Read] private TMP_Text[] playerName;
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        playerName = new TMP_Text[3];
        playerName[1] = childs.First(tr => tr.name == "PlayerSlot Text (1)").GetComponent<TMP_Text>();
        playerName[0] = childs.First(tr => tr.name == "PlayerSlot Text (2)").GetComponent<TMP_Text>();
        playerName[2] = childs.First(tr => tr.name == "PlayerSlot Text (3)").GetComponent<TMP_Text>();
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
            SetSlot(i, playerInfo.PlayerName.Value);
        }
        
        for (int i = players.Count; i < 3; ++i)
            ClearSlot(i);
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
                SetSlot(i, playerInfo.PlayerName.Value);
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

    private void SetSlot(int i, string str)
    {
        
        playerName[i].text = str;
    }
    public void WaitSlot(int i)
    {
        playerName[i].text = "Wait";
    }
    private void ClearSlot(int i)
    {
        playerName[i].text = string.Empty;
    }
}
