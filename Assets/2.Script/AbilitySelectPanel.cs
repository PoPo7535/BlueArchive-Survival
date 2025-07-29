using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;
using Random = UnityEngine.Random;

public class AbilitySelectPanel : MonoBehaviour, ISetInspector
{
    [Read, Serial] private CanvasGroup _cg;
    [Read, Serial] private Image _timerBar;
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
        _timerBar = childs.First(tr => tr.name == "TimerBarFillAmount").GetComponent<Image>();
    }

    public void Open(bool active)
    {
        _cg.ActiveCG(active);
        foreach (var selectCard in _selectCards)
        {
            selectCard.UpdateCard();
        }
    }

    public void Update()
    {
        if (Mathf.Approximately(0, _cg.alpha))
            return;
        var time = BattleSceneManager.I.SelectTimer.RemainingTime(App.I.Runner) / BattleSceneManager.I.selectTime;
        if (null != time)
        {
            if (0 != time)
                _timerBar.fillAmount = (float)time;
            else
                _selectCards[Random.Range(0, 2)].Select();
        }
    }

}
