using System;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class CharSelectPanel : MonoBehaviour, ISetInspector
{
    [Read, Serial] private GridLayoutGroup gridLayout;
    [Read, Serial] private RectTransform rt;
    [Read, Serial] private Image[] Images;
    [Read, Serial] private Button[] buttons;
    [Read, Serial] private TMP_Text[] texts;
    [Read, Serial, Fold("Size")] private Vector2 gridSize;
    [Serial, Fold("Size")] private float sizeOff;
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        rt = GetComponent<RectTransform>();
        Images = GetComponentsInChildren<Image>().Skip(1).ToArray();
        buttons = GetComponentsInChildren<Button>();
        texts = GetComponentsInChildren<TMP_Text>();
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    public void Start()
    {
        var activeColum  = Set();
        var count = gridLayout.constraintCount;
        var xCount = activeColum <= count ? count : activeColum;
        var yCount = activeColum % count;
        var xSpacingSize = gridLayout.spacing.x * count - 1;
        var ySpacingSize = gridLayout.spacing.y * count - 1;
        var xSize = xCount * gridLayout.cellSize.x + xSpacingSize + sizeOff;
        var ySize = yCount * gridLayout.cellSize.y + ySpacingSize + sizeOff;
        gridSize = new Vector2(xSize, ySize);
    }

    private int Set()
    {
        var arr = PlayableChar.None.ToArray();
        var num = 0;
        for (var i = 0; i < buttons.Length; ++i)
        {
            buttons[i].gameObject.SetActive(i < arr.Length - 1);

            if (false == i < arr.Length - 1)
                continue;
            
            ++num;
            texts[i].text = arr[i + 1].ToString();
            Images[i].sprite = LoadHelper.LoadPortrait<Sprite>(arr[i + 1].ToString());
            var a = i + 1;
            buttons[i].onClick.AddListener(() =>
            {
                LobbyRoomPanel.I.RPC_SetChar(arr[a]);
            });
        }

        return num;
    }

    public void Open(float duration = 0.3f)
    {
        rt.DOSizeDelta(gridSize, duration);
        BG.GetBG(rt.transform, () => { Close(); });
    }

    private void Close(float duration = 0.3f)
    {
        rt.DOSizeDelta(new Vector2(0, 0), duration);
    }
}
