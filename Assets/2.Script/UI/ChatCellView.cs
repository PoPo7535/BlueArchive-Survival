using System.Linq;
using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using TMPro;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class ChatCellView : EnhancedScrollerCellView, ISetInspector
{
    [Serial, Read]private TMP_Text text;
    
    public void SetData(ChatPanel.ChatData chatData)
    {
        text.alignment = chatData.notice ? TextAlignmentOptions.Midline : TextAlignmentOptions.MidlineLeft;
        text.text = chatData.text;
    }
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        text = childs.First(tr => tr.name == "ChatText").GetComponent<TMP_Text>();
    }
}
