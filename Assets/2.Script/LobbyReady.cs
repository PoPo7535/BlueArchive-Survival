using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class LobbyReady : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    
    [Serial, Read] private Button readyBtn;
    [Serial, Read] private TMP_Text readyText;

    private readonly Dictionary<PlayerRef, bool> ready = new();
    private bool isReady;    
    
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        readyBtn = childs.First(tr => tr.name == "Ready Btn").GetComponent<Button>();
        readyText = childs.First(tr => tr.name == "Ready Text").GetComponent<TMP_Text>();
    }
    
    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.LocalPlayer != player)
        {
            ready.Add(player, false);
            RefreshStartBtn();
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.LocalPlayer != player)
        {
            ready.Remove(player);
            RefreshStartBtn();
        }
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
    }
}
