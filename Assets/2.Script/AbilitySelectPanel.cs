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
    [Read, Serial] private CanvasGroup cg;
    [Read, Serial] private Button[] buttons;
    private bool _selectCard = false;
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        cg = GetComponent<CanvasGroup>();
        var childs = transform.GetAllChild();
        buttons = new Button[3];
        buttons[0] = childs.First(tr => tr.name == "Button (0)").GetComponent<Button>();
        buttons[1] = childs.First(tr => tr.name == "Button (1)").GetComponent<Button>();
        buttons[2] = childs.First(tr => tr.name == "Button (2)").GetComponent<Button>();
    }


    public void Start()
    {
        buttons[0].onClick.AddListener( () =>
        {
            Ready();
        });
        buttons[1].onClick.AddListener(() =>
        {
            Ready();
        });
        buttons[2].onClick.AddListener(() =>
        {
            Ready();
        });

        void Ready()
        {
            BattleSceneManager.I.Rpc_Ready();
            _selectCard = true;
        }
    }

    public void SetCard()
    {
        _selectCard = false;
    }
    public void Open(bool active)
    {
        cg.ActiveCG(active);
        Foo();
    }

    private async void Foo()
    {
        await new WaitUntil(() => 
            BattleSceneManager.I.SelectTimer.ExpiredOrNotRunning(App.I.Runner) || 
            _selectCard);
        if (false == _selectCard)
            BattleSceneManager.I.Rpc_Ready();
        _selectCard = false;
    }
}
