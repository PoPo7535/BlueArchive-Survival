using System.Linq;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class SpineActiveHelper : MonoBehaviour, ISetInspector
{
    [Serial, Read]private SkeletonAnimation arona;
    [Serial, Read]private SkeletonAnimation ARONA;

    [Serial, Read]private string[] names = new[] { "Idle_00", "Idle_01", "Idle_02" };
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        arona = childs.First(tr => tr.name == "Arona").GetComponent<SkeletonAnimation>();
        ARONA = childs.First(tr => tr.name == "A.R.O.N.A").GetComponent<SkeletonAnimation>();
    }
    
    [Button]
    void Start()
    {
        var ra = Random.Range(0, 2);
        var range = Random.Range(0, 3);
        if (ra == 0)
        {
            arona.gameObject.SetActive(false);
            ARONA.gameObject.SetActive(true);
            ARONA.AnimationName = names[range];
        }
        else
        {
            ARONA.gameObject.SetActive(false);
            arona.gameObject.SetActive(true);
            arona.AnimationName = names[range];
        }
    }

    
}
