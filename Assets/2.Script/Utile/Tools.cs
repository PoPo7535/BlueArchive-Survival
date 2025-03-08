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
    
    [Fold("UI_Tools")]
    [Button,GUIColor(0, 1, 0)]
    void SetCanvasGroupBlockRayCast()
    {
        var canvasGroups = FindObjectsOfType<CanvasGroup>();
        foreach (var canvas in canvasGroups)
        {
            canvas.blocksRaycasts = canvas.alpha != 0;
        }
    }
    
    [Fold("UI_Tools")]
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
    
    [Fold("UI_Tools")]
    [Button,GUIColor(0, 1, 0)]
    void SetText()
    {
        var objs = FindObjectsOfType<TMP_Text>();
        foreach (var obj in objs)
        {
            obj.fontSizeMax = 40;
        }
    }

    [Fold("AnimationClipTool")]
    [Button]
    void ChangeClipRoot()
    {
        ReplacePartialNameInClip();
    }
    [Fold("AnimationClipTool")] public AnimationClip clip;
    [Fold("AnimationClipTool")] public string oldName;
    [Fold("AnimationClipTool")] public string newName;

    void ReplacePartialNameInClip()
    {
        // 해당 애니메이션 클립의 모든 커브(키프레임)를 확인
        var bindings = AnimationUtility.GetCurveBindings(clip);

        foreach (var binding in bindings)
        {
            // 경로에서 "CH0101_Mesh" 부분을 "New_Mesh"로 변경
            if (binding.path.Contains(oldName))
            {
                // 새 경로 생성: "CH0101_Mesh/SomeOtherObject" => "New_Mesh/SomeOtherObject"
                string newPath = binding.path.Replace(oldName, newName);

                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                // 해당 키프레임을 새 경로로 설정
                var newBinding = new EditorCurveBinding
                {
                    path = newPath,
                    type = binding.type,
                    propertyName = binding.propertyName
                };

                // 새 경로에 커브를 설정
                AnimationUtility.SetEditorCurve(clip, newBinding, curve);

                // 이전 경로의 커브를 삭제 (불필요한 커브 제거)
                AnimationUtility.SetEditorCurve(clip, binding, null);
                Debug.Log($"경로 '{binding.path}'이(가) '{newPath}'으로 변경되었습니다.");
            }
        }
        EditorUtility.SetDirty(clip);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(clip));
    }
#endif
}
