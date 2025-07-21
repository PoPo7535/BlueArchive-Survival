using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public abstract class TimeStopObj : MonoBehaviour
{
    public bool CanMove => BattleSceneManager.I.StopObj;
}
