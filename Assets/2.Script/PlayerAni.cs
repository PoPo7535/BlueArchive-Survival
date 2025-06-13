using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerAni : MonoBehaviour, ISetInspector
{
    [Read] public Animator ani;
    public AnimationClip[] animClips;
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        ani = GetComponent<Animator>();
    }

    private void Awake()
    {
        SetAnimatorController();
    }

    private void SetAnimatorController()
    {
        var baseController = ani.runtimeAnimatorController;
        var overrideController = new AnimatorOverrideController(baseController);
        
        foreach (var  clip in animClips)
            overrideController[$"Base_{clip.name.Split('_')[1]}"] = clip;
        
        ani.runtimeAnimatorController = overrideController;
    }
}
