using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class MapTest : FusionSingleton<MapTest>
{
    public Image spr;
    [Networked, OnChangedRender(nameof(Ch))] private float ex {set; get;}
    public void Ch()
    {
        
        spr.fillAmount = ex/100f;
        if (100 <= ex)
        {
            ex -= 100f;
            Debug.Log(123123);
        }
    }
}
