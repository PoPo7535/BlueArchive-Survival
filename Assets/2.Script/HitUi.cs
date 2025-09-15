using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class HitUi : MonoBehaviour
{
    [Serial] private TMP_Text _text;
    private readonly float ShowTextTime = 0.3f;
    private readonly float Speed = 3;
    private float _currentTime = 0;
    

    public void SetText(string text)
    {
        _text.text = text;        
    }

    public void LateUpdate()
    {
        _currentTime += Time.deltaTime;
        transform.Translate( Speed * Time.deltaTime * Vector3.up);

        if (_currentTime < ShowTextTime) 
            return;
        _currentTime = 0;
        gameObject.SetActive(false);
    }
}
