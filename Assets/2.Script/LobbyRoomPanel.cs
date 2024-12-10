using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

public class LobbyRoomPanel : FusionSingleton<LobbyRoomPanel>, ISetInspector, IPlayerJoined
{
    public CanvasGroup cg;
    [SerializeField] private Button readyBtn;
    [SerializeField] private Button cancelBtn;
    public PlayerSlot playerSlot;
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        cg = GetComponent<CanvasGroup>();
        readyBtn = childs.First(tr => tr.name == "Ready Btn").GetComponent<Button>();
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
        var runner = App.I.runner;
        var obj = runner.Spawn(GameManager.I.playerInfo, Vector3.zero, Quaternion.identity, player,onBeforeSpawned:
            (y,r) =>
            {
                
            });
        runner.SetPlayerObject(player, obj.Object);
    }

    
    public void SetSlot()
    {
        var list = Object.Runner.ActivePlayers.ToList();
        list.Sort((p1, p2) => p1.PlayerId.CompareTo(p2.PlayerId));
        for (int i = 0; i < list.Count; ++i)
        {
            playerSlot.SetSlot(i, Object.Runner.GetPlayerObject(list[i]).GetComponent<PlayerInfo>().PlayerName.Value);
        }
    }
}
