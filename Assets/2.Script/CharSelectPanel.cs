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
    [Read, Serial] private RectTransform rt;
    [Read, Serial] private Button[] buttons;
    [Read, Serial] private TMP_Text[] texts;
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        rt = GetComponent<RectTransform>();
        buttons = GetComponentsInChildren<Button>();
        texts = GetComponentsInChildren<TMP_Text>();
    }

    public void Start()
    {
        // Close(0);
        Set();
    }

    [Button]
    public void Set()
    {
        var arr = Enum.GetValues(typeof(PlayableChar)).Cast<PlayableChar>().ToArray();

        for (var i = 0; i < buttons.Length; ++i)
        {
            buttons[i].gameObject.SetActive(i < arr.Length - 1);

            if (i < arr.Length - 1)
            {
                texts[i].text = arr[i + 1].ToString();
                var a = i + 1;
                buttons[i].onClick.AddListener(() =>
                {
                    LobbyRoomPanel.I.RPC_SetChar(arr[a]);
                });
            }
        }
    }

    [Button]
    public void Open()
    {
        rt.DOSizeDelta(new Vector2(400, 400), 0.3f);
    }

    [Button]
    public void Close(float duration = 0.3f)
    {
        rt.DOSizeDelta(new Vector2(0, 0), duration);
    }
}
