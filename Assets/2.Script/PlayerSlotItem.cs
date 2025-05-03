using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerSlotItem : MonoBehaviour, ISetInspector
{
    [Serial, Read] private TMP_Text playerName;
    [Serial, Read] private GameObject emptyArea;
    [Serial, Read] private GameObject characterView;
    [Serial, Read] private GameObject readyObj;
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        playerName = childs.First(tr => tr.name == "PlayerSlot Text").GetComponent<TMP_Text>();
        emptyArea = childs.First(tr => tr.name == "Empty Area").gameObject;
        characterView = childs.First(tr => tr.name == "View").gameObject;
        readyObj = childs.First(tr => tr.name == "Ready Text").gameObject;
    }

    public void SetSlot(string str)
    {
        emptyArea.SetActive(false);
        characterView.SetActive(true);
        playerName.text = str;
    }

    public void ClearSlot()
    {
        emptyArea.SetActive(true);
        characterView.SetActive(false);
        readyObj.SetActive(false);
        playerName.text = string.Empty;
    }

    public void SetReady(bool isReady)
    {
        readyObj.SetActive(isReady);
    }
}
