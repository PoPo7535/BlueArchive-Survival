using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using TMPro;
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
    [Read, Serial, Fold("Inspector")] private TMP_Text _msg;
    [Read, Serial] public int index;
    [Read] private static SkillType[] num = new SkillType[3];
    
    private static PlayerSkillManager _skillManager;
    private static PlayerSkillManager SkillManager
    {
        get
        {
            if (null == _skillManager)
                _skillManager = App.I.GetPlayerInfo(App.I.Runner.LocalPlayer).PlayerObject.SkillManager;
            return _skillManager;
        }
    }

    private SkillType myCard
    {
        get => num[index];
        set
        {
            num[index] = value;
            if (value == SkillType.None)
                return;
            var skillScriptable = GameManager.I.GetSkillScriptable(value);
            _selectImg.sprite = skillScriptable.Sprite;
            _msg.text = skillScriptable.msg;
        }
    }


    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        _reloadBtn = childs.First(tr => tr.name == "Reload Btn").GetComponent<Button>();
        _selectBtn = childs.First(tr => tr.name == "Select Btn").GetComponent<Button>();
        _selectImg = childs.First(tr => tr.name == "Skill Img").GetComponent<Image>();
        _msg = childs.First(tr => tr.name == "Skill Msg").GetComponent<TMP_Text>();
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
        myCard = SkillManager.GetRandomSkill();
    }

    public void Select()
    {
        if (myCard == SkillType.None)
            return;
        myCard = SkillType.None;
        BattleSceneManager.I.Rpc_SelectReady();
        SkillManager.RPC_GetSkill(SkillType.Wheel);
    }
}
