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
            if (_hitUiQueue.TryDequeue(out var textUi))
            {
                if (textUi.IsUnityNull())
                    continue;
                if (textUi.gameObject.activeSelf)
                {
                    _hitUiQueue.Enqueue(textUi);   
                    break;
                }

                textUi.gameObject.SetActive(true);
                _hitUiQueue.Enqueue(textUi);
                return textUi;
            }
            break;
        }
        _hitUiQueue.Enqueue(Object.Instantiate(GameManager.I.textUi));
        return _hitUiQueue.Peek();
    }
}
