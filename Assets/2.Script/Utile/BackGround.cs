using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public static class BG
{
    private static readonly List<GameObject> objects = new();
    public static void GetBG(Transform tr, Action action)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i].SafeIsUnityNull())
                objects.RemoveAt(i);
        }
        
        var obj = objects.Find((obj) => obj.activeSelf == false);
        if (obj.IsUnityNull())
        {
            obj = new GameObject(nameof(BG));
            objects.Add(obj);
        }
        else
            obj.SetActive(true);

        var image = obj.GetOrAddComponent<Image>();
        var button = obj.GetOrAddComponent<Button>();
        image.color = new Color(0.15f, 0.15f, 0.15f, 0.6f);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            action?.Invoke();
            obj.SetActive(false);
        });

        ((RectTransform)obj.transform).SetParent(tr.parent, new Vector2(0,0), new Vector2(1,1));
        var siblingIndex = tr.GetSiblingIndex();
        obj.transform.SetSiblingIndex(siblingIndex == 0 ? 0 : siblingIndex);
    }
}
