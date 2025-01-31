using System;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerSlot : MonoBehaviour, ISetInspector
{
    [Serial, Read] private TMP_Text[] playerName;
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        playerName = new TMP_Text[3];
        playerName[1] = childs.First(tr => tr.name == "PlayerSlot Text (1)").GetComponent<TMP_Text>();
        playerName[0] = childs.First(tr => tr.name == "PlayerSlot Text (2)").GetComponent<TMP_Text>();
        playerName[2] = childs.First(tr => tr.name == "PlayerSlot Text (3)").GetComponent<TMP_Text>();
    }

    public void Start()
    {
        foreach (var player in App.I.runner.ActivePlayers)
        {
            App.I.runner.GetPlayerObject(player);
        }
    }

    public void SetSlot(int i, string str)
    {
        playerName[i].text = str;
    }
    public void WaitSlot(int i)
    {
        playerName[i].text = "Wait";
    }
    public void ClearSlot(int i)
    {
        playerName[i].text = string.Empty;
    }
}
