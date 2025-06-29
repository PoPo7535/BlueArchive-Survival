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


public class ExpObject : SimulationBehaviour
{
    private PlayerBase _target;
    private Vector3 _targetPos;
    private Vector3 _startPos;
    private float _time;
    private bool _triggerCheck = false;
    private const float FollowingTime = 1f;
    private void OnEnable()
    {
        _time = 0;
        _triggerCheck = false;
    }

    public  void Update()
    {
        if (false == _triggerCheck)
            return;
        
        _targetPos = _target.transform.position;
        _time += Time.deltaTime;
        
        var easedValue = DOVirtual.EasedValue(0f, 1f, _time / FollowingTime, Ease.InOutBack);
        transform.position = Vector3.LerpUnclamped(_startPos, _targetPos, easedValue);
        
        if (1f <= easedValue)
            Foo();
    }

    public void Foo()
    {
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_triggerCheck)
            return;
        if (false == other.CompareTag("Player"))
            return;
        var playerBase = other.GetComponent<PlayerBase>();
        _target = playerBase;
        _startPos = transform.position;
        _triggerCheck = true;
    }
}
