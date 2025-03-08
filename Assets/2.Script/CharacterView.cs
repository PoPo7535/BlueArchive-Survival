using System.Linq;
using Fusion.Addons.Physics;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class CharacterView : MonoBehaviour, ISetInspector
{
    public GameObject[] playableChar;
    [Read, Serial]public CharacterViewItem[] views;
    
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        views = new CharacterViewItem[3];
        views[0] = childs.First(tr => tr.name == "View (2)").GetComponent<CharacterViewItem>();
        views[1] = childs.First(tr => tr.name == "View (1)").GetComponent<CharacterViewItem>();
        views[2] = childs.First(tr => tr.name == "View (3)").GetComponent<CharacterViewItem>();
    }
    
    [Button]
    public void Test(int num, PlayableChar ch)
    {
        views[num].SetChar(ch);
    }

}
