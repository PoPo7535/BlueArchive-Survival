using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class LobbyRoomPanel : FusionSingleton<LobbyRoomPanel>, ISetInspector, IPlayerJoined, IPlayerLeft
{
    public CanvasGroup cg;
    [Serial, Read] private Button readyBtn;
    [Serial, Read] private TMP_Text readyText;
    [Serial, Read] private Button cancelBtn;
    public PlayerSlot playerSlot;
    
    private Dictionary<PlayerRef, bool> ready = new Dictionary<PlayerRef, bool>();

    private bool isReady;    
    
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        cg = GetComponent<CanvasGroup>();
        readyBtn = childs.First(tr => tr.name == "Ready Btn").GetComponent<Button>();
        readyText = childs.First(tr => tr.name == "Ready Text").GetComponent<TMP_Text>();
        cancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
        playerSlot = childs.First(tr => tr.name == "PlayerSlot").GetComponent<PlayerSlot>();
    }
    private void Start()
    {
        cancelBtn.onClick.AddListener(async () =>
        {
            await App.I.runner.Shutdown(false);
            SceneManager.LoadScene("1.Lobby");
        });

    }

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log(Object.Runner.ActivePlayers.Count());
        if (Object.HasStateAuthority)
            App.I.runner.Spawn(GameManager.I.playerInfo, Vector3.zero, Quaternion.identity, player);

        if (Runner.LocalPlayer != player)
        {
            ready.Add(player, false);
            RefreshStartBtn();
        }

        SetSlot(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.LocalPlayer != player)
        {
            ready.Remove(player);
            RefreshStartBtn();
        }
        
        SetSlot(player);
    }

    private async void SetSlot(PlayerRef player)
    {
        playerSlot.WaitSlot(Object.Runner.ActivePlayers.Count() - 1);
        await UniTask.WaitUntil(() => null != Object.Runner.GetPlayerObject(player) ||
                                      default == Object.Runner.ActivePlayers.First(p => p == player));

        if (null != Object.Runner.GetPlayerObject(player))
        {
            var info = Object.Runner.GetPlayerObject(player).GetComponent<PlayerInfo>();
            await UniTask.WaitUntil(() => info.PlayerName != string.Empty);
            SetSlot();
        }

        if (PlayerRef.None == Object.Runner.ActivePlayers.First(p => p == player))
        {
            Debug.Log(2);   
            playerSlot.ClearSlot(Object.Runner.ActivePlayers.Count());
        }
    }

    private void SetSlot()
    {
        var players = Object.Runner.ActivePlayers.ToList();
        players.Sort((p1, p2) => p1.PlayerId.CompareTo(p2.PlayerId));
        for (int i = 0; i < players.Count; ++i)
        {
            var playerInfo = Object.Runner.GetPlayerObject(players[i]).GetComponent<PlayerInfo>();
            playerSlot.SetSlot(
                i, 
                playerInfo.PlayerName.Value);
        }
        
        for (int i = players.Count; i < 3; ++i)
            playerSlot.ClearSlot(i);
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            readyText.text = "Start";
            readyBtn.onClick.AddListener(() =>
            {
                App.I.runner.LoadScene("3.Battle");
            });
        }
        else
        {
            readyText.text = "Ready";
            readyBtn.image.color = isReady ? Color.white : Color.gray;

            readyBtn.onClick.AddListener(() =>
            {
                isReady = false == isReady;
                readyBtn.image.color = isReady ? Color.white : Color.gray;
                RPC_InputReadyBtn(Runner.LocalPlayer, isReady);
            });
        }
        SetSlot();

    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    private void RPC_InputReadyBtn(PlayerRef playerRef, bool isOn)
    {
        ready[playerRef] = isOn;
        RefreshStartBtn();
    }

    private void RefreshStartBtn()
    {
        if (ready.Count == 0)
            readyBtn.interactable = true;
        else
            readyBtn.interactable = false == ready.Values.Any(value => false == value);
    }

}
