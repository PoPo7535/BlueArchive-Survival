using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;


public static class Extensions
{
    public static List<Transform> GetAllChild(this Transform transform)
    {
        Queue<Transform> queue = new();
        List<Transform> list = new();
        queue.Enqueue(transform);
        
        while (0 != queue.Count)
        {
            var tr = queue.Dequeue();
            list.Add(tr);
            for (int i = 0; i<tr.childCount; ++i)
            {
                queue.Enqueue(tr.GetChild(i));
            }
        }
        return list;
    }
    public static Tween ActiveCG(this CanvasGroup _cg, bool _isShow, float _duration = 0.3f, Action _start = null,
        Action _complete = null)
    {
        return DOTween.To(() => _cg.alpha, x => _cg.alpha = x, _isShow ? 1 : 0, _duration)
            .OnStart(() =>
            {
                _start?.Invoke();

                _cg.alpha = _isShow ? 0 : 1;
                _cg.blocksRaycasts = false;
                // _cg.interactable = false;
            })
            .OnComplete(() =>
            {
                _cg.blocksRaycasts = _isShow;
                // _cg.interactable = _isShow;
                _complete?.Invoke();
            });
    }
    
    public static void SetParent(this RectTransform rect, Transform parent, Vector2 anchorMin, Vector2 anchorMax)
    {
        var pos = rect.anchoredPosition;
        var size = rect.sizeDelta;
        rect.SetParent(parent);
        rect.anchorMax = anchorMax;
        rect.anchorMin = anchorMin;
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;
    }
}
