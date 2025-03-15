using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class ChatPanel : NetworkBehaviour, IEnhancedScrollerDelegate, ISetInspector
{
    [Serial, Read] private EnhancedScroller scroller;
    [Serial, Read] private TMP_InputField chatIF;
    [Serial, Read] private EnhancedScrollerCellView chatCellView;
    private float totalCellSize;
    [Serial, Read] private List<ChatData> data = new(); 
    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        chatIF = childs.First(tr => tr.name == "Chat IF").GetComponent<TMP_InputField>();
        scroller = childs.First(tr => tr.name == "Scroll View").GetComponent<EnhancedScroller>();
        chatCellView = childs.First(tr => tr.name == "ChatCellView").GetComponent<EnhancedScrollerCellView>();
    }

    public void Start()
    {
        scroller.Delegate = this;
        scroller.ReloadData();
        data.Add(new ChatData(string.Empty, default)
        {
            size = 1000f
        });

        chatIF.onSubmit.AddListener((chat) =>
        {
            $"onSubmit {chat}".Log();
            chatIF.text = string.Empty;
        });
    }

    public void Update()
    {

    }
    [Button]
    public void SendChat(string chat, RpcInfo info = default)
    {
        Send(chat, info);
    }
    
    [Rpc(RpcSources.All,RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendChat(string chat, RpcInfo info = default)
    {
        Send(chat, info);
    }

    private void Send(string chat, RpcInfo info = default)
    {
        data.Add(new ChatData(chat, info.Source));
        ResizeScroller();
        scroller.JumpToDataIndex(data.Count - 1, 1f, 1f, tweenType: EnhancedScroller.TweenType.easeInOutSine, tweenTime: 0.5f, jumpComplete: ResetSpacer);
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


    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return data[dataIndex].size;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellView = scroller.GetCellView(chatCellView) as ChatCellView;
        cellView.SetData(data[dataIndex]);
        return cellView;
    }
    
    [Serializable]
    public class ChatData
    {
        public ChatData(string text, PlayerRef other)
        {
            this.text = text;
            this.other = other;
        }
        public string text;
        public PlayerRef other;
        public float size = 100f;
    }

}


