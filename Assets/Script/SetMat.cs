using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SetMat : MonoBehaviour
{

    public Material mat;
    
    [Button]
    public void SetMatt()
    {
        var renderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in renderers)
        {
            meshRenderer.material = mat;
        }
        var SkinnedMeshRenderer = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var meshRenderer in SkinnedMeshRenderer)
        {
            meshRenderer.material = mat;
        }
    }
    [Button]
    public void AddCollider()
    {
        var renderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in renderers)
        {
            if (false == meshRenderer.gameObject.TryGetComponent<MeshCollider>(out _))
            {
                meshRenderer.gameObject.AddComponent<MeshCollider>();
            }
        }
        var SkinnedMeshRenderer = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var meshRenderer in SkinnedMeshRenderer)
        {
            if (false == meshRenderer.gameObject.TryGetComponent<MeshCollider>(out _))
            {
                meshRenderer.gameObject.AddComponent<MeshCollider>();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
