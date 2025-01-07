using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public static class BG
{
    private static readonly List<GameObject> objects = new();
    private static Image image;
    private static Button button;
    public static void GetBG(Transform tr, Action action)
    {
        foreach (var destroyObj in objects.Where(destroyObj => destroyObj.IsUnityNull()))
        {
            objects.Remove(destroyObj);
        }
        
        var obj = objects.Find((obj) => obj.activeSelf == false);
        if (obj.IsUnityNull())
        {
            obj = new GameObject(nameof(BG));
            objects.Add(obj);
            image = obj.AddComponent<Image>();
            button = obj.AddComponent<Button>();
        }

        obj.SetActive(true);
        var rect = (RectTransform)obj.transform;
        obj.transform.parent = tr.parent;
        var siblingIndex = tr.GetSiblingIndex();
        obj.transform.SetSiblingIndex(siblingIndex == 0 ? 0 : siblingIndex - 1);
        image.color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
        button.onClick.AddListener(() =>
        {
            action?.Invoke();
            obj.SetActive(false);
        });
    }
}
