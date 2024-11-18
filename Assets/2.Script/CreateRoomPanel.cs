using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class CreateRoomPanel : MonoBehaviour, ISetInspector, INetworkRunnerCallbacks
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private LobbyRoomPanel lobbyRoomPanel;
    

    [SerializeField, FoldoutGroup("UIs")] private Toggle singleToggle;
    [SerializeField, FoldoutGroup("UIs")] private Toggle multiToggle;
    [SerializeField, FoldoutGroup("UIs")] private TMP_InputField roomNameIF;
    
    [SerializeField, FoldoutGroup("UIs")] private Toggle passwordToggle;
    [SerializeField, FoldoutGroup("UIs")] private TMP_InputField passwordIF;
    
    [SerializeField, FoldoutGroup("UIs")] private Button createBtn;
    [SerializeField, FoldoutGroup("UIs")] private Button cancelBtn;

    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.parent.GetAllChild();
        cg = gameObject.GetComponent<CanvasGroup>();
        lobbyRoomPanel = childs.First(tr => tr.name == "LobbyRoomPanel").GetComponent<LobbyRoomPanel>();
        singleToggle = childs.First(tr => tr.name == "Single Toggle").GetComponent<Toggle>();
        multiToggle = childs.First(tr => tr.name == "Multi Toggle").GetComponent<Toggle>();
        roomNameIF = childs.First(tr => tr.name == "RoomName IF").GetComponent<TMP_InputField>();
        passwordToggle = childs.First(tr => tr.name == "Password Toggle").GetComponent<Toggle>();
        passwordIF  = childs.First(tr => tr.name == "Password IF").GetComponent<TMP_InputField>();
        createBtn = childs.First(tr => tr.name == "Create Btn").GetComponent<Button>();
        cancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
    }

    private void Start()
    {
        App.I.runner.AddCallbacks(this);

        #region SetButtons
        singleToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
                return;
            roomNameIF.interactable = false;
            roomNameIF.text = string.Empty;
            passwordToggle.isOn = false;
            passwordToggle.interactable = false;
        });
        multiToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
                return;
            roomNameIF.interactable = true;
            passwordToggle.interactable = true;
        });
        passwordToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
                passwordIF.text = string.Empty;
            passwordIF.interactable = isOn;
        });
        
        createBtn.onClick.AddListener(() =>
        {
            if (passwordToggle.isOn && passwordIF.text.IsNullOrEmpty())
            {
                PopUp.I.OpenPopUp(
                    "비밀번호 입력",
                    () => PopUp.I.ActiveCG(false),
                    "닫기");
                return;
            }

            App.I.HostGame(
                singleToggle.isOn ? GameMode.Single : GameMode.Host,
                passwordIF.text.IsNullOrEmpty() ? int.MaxValue : int.Parse(passwordIF.text),
                () =>
                {
                    cg.ActiveCG(false);
                    lobbyRoomPanel.cg.ActiveCG(true);
                });
        });
        
        cancelBtn.onClick.AddListener(() =>
        {
            cg.ActiveCG(false);
        });
        #endregion
    }

    private void OnDestroy()
    {
        App.I.runner.AddCallbacks(this);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        var obj = App.I.runner.Spawn(playerInfo, Vector3.zero, Quaternion.identity, player);
        App.I.runner.SetPlayerObject(player, obj.Object);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        App.I.runner.Despawn(App.I.runner.GetPlayerObject(player));
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
