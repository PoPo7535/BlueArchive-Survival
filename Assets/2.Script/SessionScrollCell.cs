using EnhancedScrollerDemos.SuperSimpleDemo;
using EnhancedUI.EnhancedScroller;
using TMPro;

public class SessionScrollCell : EnhancedScrollerCellView
{
    /// <summary>
    /// A reference to the UI Text element to display the cell data
    /// </summary>
    public TMP_Text someTextText;

    /// <summary>
    /// This function just takes the Demo data and displays it
    /// </summary>
    /// <param name="data"></param>
    public void SetData(Data data)
    {
        // update the UI text with the cell data
        someTextText.text = data.someText;
    }
}
