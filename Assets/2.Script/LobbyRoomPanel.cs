using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Serial, Read] private PlayerSlot playerSlot;

    private PlayerInfo localPlayerInfo => Object.Runner.GetPlayerObject(Object.Runner.LocalPlayer).GetComponent<PlayerInfo>();
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
        if (Object.HasStateAuthority)
            App.I.runner.Spawn(GameManager.I.playerInfo, Vector3.zero, Quaternion.identity, player);

        if (Runner.LocalPlayer != player)
        {
            ready.Add(player, false);
            RefreshStartBtn();
        }

        playerSlot.SetPlayerSlot(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.LocalPlayer != player)
        {
            ready.Remove(player);
            RefreshStartBtn();
        }

        playerSlot.SetAllPlayerSlot();
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

        playerSlot.SetAllPlayerSlot();
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 150, 100), "카요코"))
        {
            RPC_CharIndex(1);
        }
        
        if (GUI.Button(new Rect(50, 150, 150, 100), "카요코2"))
        {
            RPC_CharIndex(2);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_CharIndex(int index)
    {
        localPlayerInfo.CharIndex = index;
        index.Log();
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
