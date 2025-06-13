using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerBase : NetworkBehaviour
{
    [Read] public PlayerAniController aniController;
    [Read] public PlayerMove playerMove;
    [Read] public PlayerStateController playerStateController;
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

    public override void Spawned()
    {
        var charIndex = App.I.GetPlayerInfo(Object.InputAuthority).CharIndex;
        var model = GameManager.I.playableChar[charIndex];
        var modelObj = Instantiate(
            model,
            transform.position,
            Quaternion.identity);
        modelObj.transform.SetParent(transform);
        transform.localScale = Vector3.one * 100;
        aniController.playerAni = modelObj.GetComponent<PlayerAni>();
    }
}


