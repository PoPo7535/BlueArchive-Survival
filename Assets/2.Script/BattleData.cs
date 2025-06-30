using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class BattleData
{
    public float curExp {  get; private set;}
    public float totalExp {  get; private set;}
    public float totalDamage {  get; private set;}


    public void AddExp(float addExp)
    {
        curExp += addExp;
        totalExp += addExp;
    }

    public void ReSetExp()
    {
        curExp = 0f;
    }
    public void ReSetData()
    {
        curExp = 0f;
        totalExp = 0f;
        totalDamage = 0f;
    }
}
