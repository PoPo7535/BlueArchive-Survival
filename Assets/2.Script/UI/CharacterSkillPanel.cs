using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class CharacterSkillPanel : LocalSingleton<CharacterSkillPanel>, ISetInspector
{
    [Serial, Read] private CanvasGroup _cg;
    [Serial, Read] private Image _portraitImg;
    [Serial, Read] private Image _charSkill1;
    [Serial, Read] private Image _charSkill2;
    [Serial, Read] private SkillGroup _passiveSkillGroup;
    [Serial, Read] private SkillGroup _activeSkillGroup;
    
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        _cg = GetComponent<CanvasGroup>();
        var childs = transform.GetAllChild();
        _portraitImg = childs.First(tr => tr.name == "Portrait Img").GetComponent<Image>();
        _activeSkillGroup = childs.First(tr => tr.name == "ActiveSkill Panel").GetComponent<SkillGroup>();
        _activeSkillGroup.GetComponent<ISetInspector>().SetInspector();
        _passiveSkillGroup = childs.First(tr => tr.name == "PassiveSkill Panel").GetComponent<SkillGroup>();
        _passiveSkillGroup.GetComponent<ISetInspector>().SetInspector();
        _charSkill1 = childs.First(tr => tr.name == "CharSKill 1").GetComponent<Image>();
        _charSkill2 = childs.First(tr => tr.name == "CharSKill 2").GetComponent<Image>();
    }

    public void SkillUpdate(Dictionary<SkillType, ActiveSkillBase> activeSkills)
    {
        _activeSkillGroup.SkillUpdate(activeSkills);
        // _passiveSkillGroup.SkillUpdate(activeSkills);
    }
}