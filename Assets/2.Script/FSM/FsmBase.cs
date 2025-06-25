using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class FsmBase : IFsmStateTarget
{
    protected IFsmStateOther other { get; private set; }

    public FsmBase(IFsmStateOther other)
    {
        this.other = other;
    }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate(Spawner.NetworkInputData data) { }
}
