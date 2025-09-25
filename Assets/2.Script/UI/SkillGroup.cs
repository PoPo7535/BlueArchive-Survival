using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class SkillGroup : MonoBehaviour,ISetInspector
{
    [Serial, Read] private Image[] _skills = new Image[6];
    
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        var list = transform.GetAllChild();

        for (int i = 0; i < _skills.Length; ++i)
        {
            _skills[i] = list.Find(o => o.name == $"SkillIcon ({i})").GetComponent<Image>();
        }
    }

    public void SkillUpdate()
    {
        
    }
}
