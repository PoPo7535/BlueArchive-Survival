using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class HitUtile 
{
    private static readonly Queue<HitUi> _hitUiQueue = new();


    public static void ShowHitLog(HitData hitData, Transform transform)
    {
        var textObj = GetTextObject();
        textObj.SetText($"{(int)hitData.damage}");
        textObj.transform.position = transform.position;
    }

    private static HitUi GetTextObject()
    {
        while (true) 
        {
            if (_hitUiQueue.TryDequeue(out var text))
            {
                if (text.IsUnityNull())
                    continue;
                if (text.gameObject.activeSelf)
                    break;

                text.gameObject.SetActive(true);
                _hitUiQueue.Enqueue(text);
                return text;
            }
            break;
        }
        _hitUiQueue.Enqueue(Object.Instantiate(GameManager.I.textUi));
        return _hitUiQueue.Peek();
    }
}
