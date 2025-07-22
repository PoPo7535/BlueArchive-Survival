using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class AbilitySelectPanel : MonoBehaviour, ISetInspector
{
    [Read, Serial] private CanvasGroup _cg;
    [Read, Serial] private SelectCard[] _selectCards;
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        _cg = GetComponent<CanvasGroup>();
        var childs = transform.GetAllChild();
        _selectCards = new SelectCard[3];
        _selectCards[0] = childs.First(tr => tr.name == "SelectCard (0)").GetComponent<SelectCard>();
        _selectCards[0].index = 0;
        _selectCards[1] = childs.First(tr => tr.name == "SelectCard (1)").GetComponent<SelectCard>();
        _selectCards[1].index = 1;
        _selectCards[2] = childs.First(tr => tr.name == "SelectCard (2)").GetComponent<SelectCard>();
        _selectCards[2].index = 2;
    }

    public void Open(bool active)
    {
        _cg.ActiveCG(active);
        foreach (var selectCard in _selectCards)
        {
            selectCard.UpdateCard();
        }
    }
}
