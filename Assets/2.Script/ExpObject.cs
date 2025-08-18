using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;
public class ExpObject : MonoBehaviour
{
    private PlayerBase _target;
    private Vector3 _targetPos;
    private Vector3 _startPos;
    private float _time;
    private bool _triggerCheck = false;
    private const float FollowingTime = 1f;
    private float addExpValue;
    private void OnEnable()
    {
        _time = 0;
        _triggerCheck = false;
    }

    public void Update()
    {
        if (false == _triggerCheck)
            return;
        
        _targetPos = _target.transform.position;
        _time += Time.deltaTime;
        
        var easedValue = DOVirtual.EasedValue(0f, 1f, _time / FollowingTime, Ease.InOutBack);
        transform.position = Vector3.LerpUnclamped(_startPos, _targetPos, easedValue);
        
        if (1f <= easedValue)
            AddExp();
    }

    public void Init(float addExpValue)
    {
        this.addExpValue = addExpValue;
    }
    public void AddExp()
    {
        gameObject.SetActive(false);
        BattleSceneManager.I.battleData.AddExp(40f);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (_triggerCheck)
            return;
        if (false == other.CompareTag("Player"))
            return;
        var playerBase = other.GetComponent<PlayerBase>();
        if (playerBase.Object.InputAuthority != App.I.Runner.LocalPlayer)
            return;
        _triggerCheck = true;
        _target = playerBase;
        _startPos = transform.position;
    }
}
