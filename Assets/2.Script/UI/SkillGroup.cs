using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class SkillGroup : MonoBehaviour,ISetInspector
{
    [Serial, Read] private Image[] _bgs = new Image[6];
    [Serial, Read] private Image[] _skillImg = new Image[6];
    [Serial, Read] private TMP_Text[] _text = new TMP_Text[6];
    
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        var list = transform.GetAllChild();

        for (int i = 0; i < _bgs.Length; ++i)
        {
            _bgs[i] = list.Find(o => o.name == $"SkillIcon ({i})").GetComponent<Image>();
            _skillImg[i] = _bgs[i].transform.GetChild(0).GetComponent<Image>();
            _text[i] = _bgs[i].transform.GetChild(1).GetComponent<TMP_Text>();
        }
    }

    public void SkillUpdate(Dictionary<SkillType, ActiveSkillBase> skills)
    {
        int i = 0;
        foreach (var pair in skills.Values)
        {
            _text[i].text = $"{pair.currentLevel}";
            _skillImg[i].sprite = GameManager.I.GetSkillScriptable(pair.type).Sprite;
            ++i;
        }
    }
}
