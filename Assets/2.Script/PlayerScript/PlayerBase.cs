using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerBase : NetworkBehaviour
{
    [NonSerialized, Read] public PlayerAniController AniController;
    [NonSerialized, Read] public PlayerFsmController FsmController;
    [NonSerialized, Read] public PlayerSkillManager SkillManager;
    [NonSerialized, Read] public PlayerState State;
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
        AniController.playerAni = modelObj.GetComponent<PlayerAni>();
    }
}


