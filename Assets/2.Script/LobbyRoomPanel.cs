using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

public class LobbyRoomPanel : Singleton<LobbyRoomPanel>, ISetInspector
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
        cancelBtn.onClick.AddListener((async () =>
        {
            await App.I.runner.Shutdown(false);
            SceneManager.LoadScene("1.Lobby");
        }));
        readyBtn.onClick.AddListener(() =>
        {
            App.I.runner.LoadScene("3.Battle");
        });
    }
}
