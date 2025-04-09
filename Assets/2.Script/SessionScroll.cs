using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class SessionScroll : MonoBehaviour, IEnhancedScrollerDelegate, INetworkRunnerCallbacks
{
    private List<SessionInfo> _data = new();
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;

    public void Start()
    {
        scroller.Delegate = this;
        scroller.ReloadData();
        App.I.runner.AddCallbacks(this);
        cellViewPrefab.gameObject.SetActive(false);
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return ((RectTransform)cellViewPrefab.transform).sizeDelta.y;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellView = scroller.GetCellView(cellViewPrefab) as SessionScrollCell;

        cellView.name = $"Cell {dataIndex}";

        cellView.SetData(_data[dataIndex]);

        return cellView;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Debug.Log(player);
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        _data = sessionList;
        scroller.ReloadData();
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
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
