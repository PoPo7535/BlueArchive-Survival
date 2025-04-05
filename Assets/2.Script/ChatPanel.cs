using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class ChatPanel : NetworkBehaviour, IEnhancedScrollerDelegate, ISetInspector, IPlayerJoined, IPlayerLeft
{
    [Serial, Read] private EnhancedScrollerCellView view;
    [Serial, Read] private EnhancedScroller scroller;
    [Serial, Read] private List<ChatData> data;
    [Serial, Read] private TMP_InputField chatIF;
    [Serial, Read] private TMP_Text helperText;

    private Dictionary<PlayerRef, string> playerNames = new();
    private float totalCellSize;

    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        chatIF = childs.First(tr => tr.name == "Chat IF").GetComponent<TMP_InputField>();
        scroller = childs.First(tr => tr.name == "Scroll View").GetComponent<EnhancedScroller>();
        view = childs.First(tr => tr.name == "ChatCellView").GetComponent<EnhancedScrollerCellView>();
        helperText = childs.First(tr => tr.name == "ChatText").GetComponent<TMP_Text>();
    }

    public void Start()
    {
        data = new List<ChatData>();
        scroller.Delegate = this;
        scroller.ReloadData();
        data.Add(new ChatData(string.Empty, default, 100, false));

        chatIF.onSubmit.AddListener((chat) =>
        {
            if (chat != string.Empty)
                RPC_SendChat(chat);
            chatIF.text = string.Empty;
        });
    }

    public async void PlayerJoined(PlayerRef player)
    {
        if (false == App.I.IsHost)
            return;
        await App.I.ConnectingPlayer(player);
        var name = App.I.GetPlayerInfo(player).PlayerName.Value;
        playerNames.Add(player, name);
        RPC_NoticeSend(player, name, true);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (false == App.I.IsHost)
            return;
        var name = playerNames[player];
        RPC_NoticeSend(player, name,false);
        playerNames.Remove(player);
    }

    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_SendChat(string chat, RpcInfo info = default)
    {
        var playerName = App.I.GetPlayerInfo(info.Source).PlayerName.Value;
        var isLocal = Runner.LocalPlayer == info.Source;
        helperText.text = isLocal
            ? $"<color=red>{playerName}</color> : {chat}"
            : $"<color=blue>{playerName}</color> : {chat}";
        data.Add(new ChatData(helperText.text, info.Source, helperText.preferredHeight, false));
        UpdateChat();
    }

    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_NoticeSend(PlayerRef player, string name, bool isJoin)
    {
        helperText.text = isJoin ? $"<color=#B4B4B4>{name} Join" : $"<color=#B4B4B4>{name} Left";
        data.Add(new ChatData(helperText.text, player, helperText.preferredHeight, true));
        UpdateChat();
    }

    private void UpdateChat()
    {
        ResizeScroller();
        scroller.JumpToDataIndex(data.Count - 1, 1f, 1f, tweenType: EnhancedScroller.TweenType.easeInOutSine,
            tweenTime: 0.5f, jumpComplete: ResetSpacer);

        void ResetSpacer()
        {
            data[0].size = Mathf.Max(scroller.ScrollRectSize - totalCellSize, 0);
            scroller.ReloadData(1.0f);
        }

        void ResizeScroller()
        {
            var scrollRectSize = scroller.ScrollRectSize;
            var offset = scroller.ScrollSize;
            var rectTransform = scroller.GetComponent<RectTransform>();
            var size = rectTransform.sizeDelta;
            rectTransform.sizeDelta = new Vector2(size.x, float.MaxValue);
            totalCellSize = scroller.padding.top + scroller.padding.bottom;
            for (var i = 1; i < data.Count; i++)
                totalCellSize += data[i].size + (i < data.Count - 1 ? scroller.spacing : 0);
            data[0].size = scrollRectSize;
            rectTransform.sizeDelta = size;
            scroller.ReloadData();
            scroller.ScrollPosition = (totalCellSize - data[^1].size) + offset;
        }
    }

    public int GetNumberOfCells(EnhancedScroller _)
    {
        return data.Count;
    }

    public float GetCellViewSize(EnhancedScroller _, int dataIndex)
    {
        return data[dataIndex].size;

    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller _, int dataIndex, int cellIndex)
    {
        var cellView = scroller.GetCellView(view) as ChatCellView;
        cellView.SetData(data[dataIndex]);

        return cellView;
    }

    [Serializable]
    public class ChatData
    {
        public ChatData(string text, PlayerRef other, float size, bool notice)
        {
            this.notice = notice;
            this.text = text;
            this.other = other;
            this.size = size;
        }

        public bool notice;
        public string text;
        public PlayerRef other;
        public float size;
    }
}

