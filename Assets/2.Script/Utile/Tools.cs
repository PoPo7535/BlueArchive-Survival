using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tools : MonoBehaviour
{
    [Button,GUIColor(0, 1, 0)]
    void SetNavigation()
    {
        var newNavigation = new Navigation
        {
            mode = Navigation.Mode.None
        };
        var buttons = FindObjectsOfType<Button>();
        foreach (var btn in buttons)
            btn.navigation = newNavigation;
        
        var toggles = FindObjectsOfType<Toggle>();
        foreach (var btn in toggles)
            btn.navigation = newNavigation;
        
        var inputFields = FindObjectsOfType<TMP_InputField>();
        foreach (var btn in inputFields)
            btn.navigation = newNavigation;
    }
    
    [Button,GUIColor(0, 1, 0)]
    void SetCanvasGroupBlockRayCast()
    {
        var canvasGroups = FindObjectsOfType<CanvasGroup>();
        foreach (var canvas in canvasGroups)
        {
            canvas.blocksRaycasts = canvas.alpha != 0;
        }
    }
    [Button,GUIColor(0, 1, 0)]
    void SetPlaceholder()
    {
        var objs = FindObjectsOfType<TMP_Text>();
        objs =  objs.Where(o => o.name == "Placeholder").ToArray();
        foreach (var obj in objs)
        {
            obj.color = new Color32(200,200,200,255);
        }
    }
    [Button,GUIColor(0, 1, 0)]
    void SetText()
    {
        var objs = FindObjectsOfType<TMP_Text>();
        foreach (var obj in objs)
        {
            obj.fontSizeMax = 40;
        }
    }
}
