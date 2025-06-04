using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerBase : MonoBehaviour
{
    [Read] public PlayerAniController aniController;
    [Read] public PlayerMove playerMove;
    public FindEnemy findEnemy;
    private void Awake()
    {
        foreach (var component in GetComponents<MonoBehaviour>())
        {
            if (component is IPlayerComponent playerComponent)
                playerComponent.Init(this);
        }

        findEnemy = new FindEnemy(transform);
    }
}


