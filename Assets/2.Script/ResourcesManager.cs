using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class ResourcesManager : MonoBehaviour, ISetInspector
{
    [Button ,GUIColor(0,1,0)]
    public void SetInspector()
    {
        
    }
}
