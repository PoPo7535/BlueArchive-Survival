using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class RealToonShaderSet : MonoBehaviour
{
    [Fold("SourceMaterial")] public Material normalSource;
    [Fold("SourceMaterial")] public Material bodySource;
    [Fold("SourceMaterial")] public Material eyeMouthSource;
    [Fold("SourceMaterial")] public Material faceSource;

    public List<Material> normalTarget;
    public List<Material> bodyTarget;
    public List<Material> eyeMouthTarget;
    public List<Material> faceTarget;


    [Fold("GetMaterial")] public SkinnedMeshRenderer targetMesh;
    [Fold("GetMaterial")][Button]
    private void GetMaterialFromSkinMesh()
    {
        foreach (var val in targetMesh.sharedMaterials)
        {
            if (val.name.Contains("Body"))
                bodyTarget.Add(val);
            else if (val.name.Contains("EyeMouth"))
                eyeMouthTarget.Add(val);
            else if (val.name.Contains("Face"))
                faceTarget.Add(val);
            else if (val.name.Contains("Mouth"))
                continue;
            else
                normalTarget.Add(val);
        }
    }
    
    [Button]
    private void SetShader()
    {
        var names = normalSource.GetPropertyNames(MaterialPropertyType.Float);
        foreach (var propertyName in names)
        {
            foreach (var value in normalTarget)
                value.SetFloat(propertyName, normalSource.GetFloat(propertyName));
            
            foreach (var value in bodyTarget)
                value.SetFloat(propertyName, bodySource.GetFloat(propertyName));
            
            foreach (var value in eyeMouthTarget)
                value.SetFloat(propertyName, eyeMouthSource.GetFloat(propertyName));
            
            foreach (var value in faceTarget)
                value.SetFloat(propertyName, faceSource.GetFloat(propertyName));
        }
    }

    [Button]
    private void Clear()
    {
        normalTarget.Clear();
        bodyTarget.Clear();
        eyeMouthTarget.Clear();
        faceTarget.Clear();
    }
}
