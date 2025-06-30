using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ExpBar : MonoBehaviour
{
    public Image expBar;

    public void SetBar(float val)
    {
        expBar.fillAmount = val;
    }

}
