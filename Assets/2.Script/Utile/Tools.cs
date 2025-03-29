using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class Tools : MonoBehaviour
{
#if UNITY_EDITOR
    [Fold("UI_Tools")]
    [Button,GUIColor(0, 1, 0)]
    private void SetNavigation()
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
    
    [Fold("UI_Tools")]
    [Button,GUIColor(0, 1, 0)]
    private void SetCanvasGroupBlockRayCast()
    {
        var canvasGroups = FindObjectsOfType<CanvasGroup>();
        foreach (var canvas in canvasGroups)
        {
            canvas.blocksRaycasts = canvas.alpha != 0;
        }
    }
    
    [Fold("UI_Tools")]
    [Button,GUIColor(0, 1, 0)]
    private void SetPlaceholder()
    {
        var objs = FindObjectsOfType<TMP_Text>();
        objs =  objs.Where(o => o.name == "Placeholder").ToArray();
        foreach (var obj in objs)
        {
            obj.color = new Color32(200,200,200,255);
        }
    }
    
    [Fold("UI_Tools")]
    [Button,GUIColor(0, 1, 0)]
    private void SetText()
    {
        var objs = FindObjectsOfType<TMP_Text>();
        foreach (var obj in objs)
        {
            obj.fontSizeMax = 40;
        }
    }
    [Fold("AnimationClipTool")] public AnimationClip clip;
    [Fold("AnimationClipTool")] public string oldName;
    [Fold("AnimationClipTool")] public string newName;
    [Fold("AnimationClipTool")]
    [Button]
    private void ChangeClipRoot()
    {
        var bindings = AnimationUtility.GetCurveBindings(clip);
        foreach (var binding in bindings)
        {
            if (!binding.path.Contains(oldName)) 
                continue;
            var newPath = binding.path.Replace(oldName, newName);

            var curve = AnimationUtility.GetEditorCurve(clip, binding);
            var newBinding = new EditorCurveBinding
            {
                path = newPath,
                type = binding.type,
                propertyName = binding.propertyName
            };

            AnimationUtility.SetEditorCurve(clip, newBinding, curve);
            AnimationUtility.SetEditorCurve(clip, binding, null);
            Debug.Log($"경로 '{binding.path}'이(가) '{newPath}'으로 변경되었습니다.");
        }
        EditorUtility.SetDirty(clip);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(clip));
    }
    [Button,GUIColor(0, 1, 0)]

    public void SetInspector()
    {
        var inter = new List<ISetInspector>();
        var allObj = FindObjectsOfType<MonoBehaviour>();
        foreach (var obj in allObj)
        {
            if (obj is ISetInspector ins)
            {
                inter.Add(ins);
            }
        }

        foreach (var inspector in inter)
        {
            inspector.SetInspector();
        }
    }
#endif
}
