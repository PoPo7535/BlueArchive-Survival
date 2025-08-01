using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class SelectCard : MonoBehaviour, ISetInspector
{
    [Read, Serial, Fold("Inspector")] private Button _reloadBtn;
    [Read, Serial, Fold("Inspector")] private Button _selectBtn;
    [Read, Serial, Fold("Inspector")] private Image _selectImg;
    [Read, Serial] public int index;
    
    [Read] private static int[] num = new int[3];
    [Read,ShowInInspector] private int myCard
    {
        get => num[index];
        set => num[index] = value;
    }


    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        _reloadBtn = childs.First(tr => tr.name == "Reload Btn").GetComponent<Button>();
        _selectBtn = childs.First(tr => tr.name == "Select Btn").GetComponent<Button>();
        _selectImg = childs.First(tr => tr.name == "Select Btn").GetComponent<Image>();
    }

    public void Start()
    {
        _reloadBtn.onClick.AddListener(() =>
        {
            UpdateCard();
        });
        _selectBtn.onClick.AddListener(() =>
        {
            Select();
        });
    }

    public void UpdateCard()
    {
        myCard = 1;
    }

    public void Select()
    {
        if (myCard == 0)
            return;
        myCard = 0;
        BattleSceneManager.I.Rpc_Ready();
    }

}
