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

public class AbilitySelectPanel : MonoBehaviour, ISetInspector
{
    [Read, Serial] public CanvasGroup cg;
    [Read, Serial] private Button[] _buttons;
    
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        cg = GetComponent<CanvasGroup>();
        var childs = transform.GetAllChild();
        _buttons = new Button[3];
        _buttons[0] = childs.First(tr => tr.name == "Button (0)").GetComponent<Button>();
        _buttons[1] = childs.First(tr => tr.name == "Button (1)").GetComponent<Button>();
        _buttons[2] = childs.First(tr => tr.name == "Button (2)").GetComponent<Button>();
    }

    public void Start()
    {
        _buttons[0].onClick.AddListener(() =>
        {
            BattleSceneManager.I.Rpc_Ready();
        });
        _buttons[1].onClick.AddListener(() =>
        {
            BattleSceneManager.I.Rpc_Ready();
        });
        _buttons[2].onClick.AddListener(() =>
        {
            BattleSceneManager.I.Rpc_Ready();
        });
        
    }
}
