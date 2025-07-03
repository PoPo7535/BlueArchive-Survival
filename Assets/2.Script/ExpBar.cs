using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ExpBar : MonoBehaviour, ISetInspector
{
    [Read] public CanvasGroup cg;
    public Image expBar;

    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public void SetBar(float val)
    {
        expBar.fillAmount = val;
    }
}
