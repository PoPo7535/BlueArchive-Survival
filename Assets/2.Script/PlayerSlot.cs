using System;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerSlot : MonoBehaviour, ISetInspector
{
    [SerializeField] private TMP_Text[] playerName;
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        playerName = new TMP_Text[3];
        playerName[0] = childs.First(tr => tr.name == "PlayerSlot Text (1)").GetComponent<TMP_Text>();
        playerName[1] = childs.First(tr => tr.name == "PlayerSlot Text (2)").GetComponent<TMP_Text>();
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
}
