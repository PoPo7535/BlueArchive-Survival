using System.Linq;
using Fusion.Addons.Physics;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class CharacterView : MonoBehaviour, ISetInspector
{
    public GameObject characterBase;
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
    public void Test()
    {
        // var obj = Instantiate(characterBase,Vector3.zero, Quaternion.identity, views[0].character.transform);
        // obj.transform.localPosition = Vector3.zero;

        var playerChar = Instantiate(playableChar[0], Vector3.zero, Quaternion.identity, views[0].character.transform);
        playerChar.transform.localPosition = Vector3.zero;
        playerChar.transform.localScale = Vector3.one * 900;
    }

    public void Test2()
    {
        var playerChar = Instantiate(playableChar[0], Vector3.zero, Quaternion.identity, views[0].character.transform);
        playerChar.transform.localPosition = Vector3.zero;
        playerChar.transform.localScale = Vector3.one * 900;
        playerChar.AddComponent<NetworkRigidbody3D>();
        // playerChar.GetComponent<CapsuleCollider>().enabled = false;
        playerChar.AddComponent<PlayerMove>().enabled = false;
    }
}
