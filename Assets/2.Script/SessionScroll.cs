using EnhancedScrollerDemos.SuperSimpleDemo;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class SessionScroll : MonoBehaviour, IEnhancedScrollerDelegate
{
    private SmallList<Data> _data;

    public EnhancedScroller scroller;

    public EnhancedScrollerCellView cellViewPrefab;

    public void Start()
    {
        scroller.Delegate = this;
        _data = new SmallList<Data>();
        for (var i = 0; i < 1000; i++)
            _data.Add(new Data() { someText = "Cell Data Index " + i.ToString() });

        // tell the scroller to reload now that we have the data
        scroller.ReloadData();
        // scroller.JumpToDataIndex(0);
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

        cellView.name = "Cell " + dataIndex;

        cellView.SetData(_data[dataIndex]);

        return cellView;
    }
}
