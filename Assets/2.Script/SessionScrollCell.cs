using EnhancedScrollerDemos.SuperSimpleDemo;
using EnhancedUI.EnhancedScroller;
using Fusion;
using TMPro;
using UnityEngine.UI;

public class SessionScrollCell : EnhancedScrollerCellView
{
    /// <summary>
    /// A reference to the UI Text element to display the cell data
    /// </summary>
    public TMP_Text someTextText;
    public Button btn;

    /// <summary>
    /// This function just takes the Demo data and displays it
    /// </summary>
    /// <param name="data"></param>
    public void SetData(SessionInfo data)
    {
        // update the UI text with the cell data
        someTextText.text = data.Name;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            App.I.ClientGame(data.Name);
        });
    }
}
