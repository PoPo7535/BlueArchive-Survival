using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

public class LobbyRoomPanel : FusionSingleton<LobbyRoomPanel>, ISetInspector, IPlayerJoined, IPlayerLeft
{
    public CanvasGroup cg;
    [SerializeField] private Button readyBtn;
    [SerializeField] private TMP_Text readyText;
    [SerializeField] private Button cancelBtn;
    public PlayerSlot playerSlot;

    private Dictionary<PlayerRef, bool> ready = new Dictionary<PlayerRef, bool>();
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
        readyBtn.onClick.AddListener(() =>
        {
            App.I.runner.LoadScene("3.Battle");
        });
    }
    
    public void PlayerJoined(PlayerRef player)
    {
        if (false == Object.HasStateAuthority)
            return;
        
        App.I.runner.Spawn(GameManager.I.playerInfo, Vector3.zero, Quaternion.identity, player);
        
        if (Object.StateAuthority != player)
            ready.Add(player, false);
        
        RefreshStartBtn();
    }
    
    public void PlayerLeft(PlayerRef player)
    {
        if (false == Object.HasStateAuthority)
            return;
        
        if (Object.StateAuthority != player)
            ready.Remove(player);
    }
    
    public void SetSlot()
    {
        var list = Object.Runner.ActivePlayers.ToList();
        list.Sort((p1, p2) => p1.PlayerId.CompareTo(p2.PlayerId));
        for (int i = 0; i < list.Count; ++i)
            playerSlot.SetSlot(i, Object.Runner.GetPlayerObject(list[i]).GetComponent<PlayerInfo>().PlayerName.Value);
        
        for (int i = list.Count; i < 3; ++i)
            playerSlot.SetSlot(i);
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            readyText.text = "Start";
        }
        else
        {
            readyText.text = "Ready";
            
        }
    }

    [Rpc(RpcSources.InputAuthority,RpcTargets.StateAuthority)]
    private void RPC_InputReadyBtn(PlayerRef playerRef, bool isOn)
    {
        ready[playerRef] = isOn;
    }

    private void RefreshStartBtn()
    {
        readyBtn.interactable = false == ready.Values.Any(value => false == value);
    }

}
