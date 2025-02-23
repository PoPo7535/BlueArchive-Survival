using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class CharacterViewItem : MonoBehaviour, ISetInspector
{
    [Serial, Read] private GameObject character;
    [Serial, Read] private GameObject camera;
    [Read] private Dictionary<string, GameObject> dic = new();

    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        character = childs.First(tr => tr.name == "Character").gameObject;
        camera = childs.First(tr => tr.name == "Camera").gameObject;
    }

}
