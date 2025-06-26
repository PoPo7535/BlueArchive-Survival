using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ExpObject : MonoBehaviour
{
    private PlayerBase target;

    private void FixedUpdate()
    {
        if (false == target)
            return;
        var dir = (target.transform.position - transform.position).normalized;
        transform.position += Time.fixedTime * 0.001f*  dir;
        var dis = Vector3.Distance(target.transform.position, transform.position);
        if (dis <= 1f)
        {
            Foo();
        }
    }

    public void Foo()
    {
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (false == other.CompareTag("Player"))
            return;
        var playerBase = other.GetComponent<PlayerBase>();
        target = playerBase;
    }
}
