using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;
public class MeshCutTool : MonoBehaviour
{
    [Fold("Base")] public Mesh modelMesh;
    [Fold("Base")] public Material mouthMat;
    [Fold("Base")] public Texture2D mouthTx;
    
    [Fold("Mesh Cut")] public SkinnedMeshRenderer meshRender;
    [Fold("Mesh Cut")] public int faceMeshIndex = 2;
    [Fold("Mesh Cut")]
    public int meshCutStartIndex = 0;
    
    [Fold("Mesh Cut")]
    public int meshCutEndIndex = 32;

    private int MeshCutStartIndex => meshCutStartIndex*3;
    private int MeshCutEndIndex => meshCutEndIndex * 3;

    [Fold("Gizmo")]
    public bool showGizmo = false;
    
    [Fold("Gizmo")]
    public float scale = 1F;
    
    [Fold("Gizmo")]
    public bool isSet;

    [Button, Fold("Mesh Cut")]
    private void MeshCut()
    {
        if (false == isSet)
            MeshCutMouth();
        isSet = true;
    }


    private void MeshCutMouth()
    {
        var subMeshDescriptor = modelMesh.GetSubMesh(faceMeshIndex);
        modelMesh.subMeshCount += 1;
        if (MeshCutStartIndex == 0)
            modelMesh.SetSubMesh(faceMeshIndex, new SubMeshDescriptor(subMeshDescriptor.indexStart + MeshCutEndIndex, subMeshDescriptor.indexCount - MeshCutEndIndex));
        else
        {
            var subMesh1 = new SubMeshDescriptor(subMeshDescriptor.indexStart, MeshCutStartIndex);
            var subMesh2 = new SubMeshDescriptor(subMeshDescriptor.indexStart + MeshCutEndIndex, subMeshDescriptor.indexCount - MeshCutEndIndex);
            
            modelMesh.SetSubMesh(faceMeshIndex, subMesh1);
            modelMesh.SetSubMesh(modelMesh.subMeshCount - 1, subMesh2);
            var materials = meshRender.sharedMaterials;
            materials[modelMesh.subMeshCount - 1] = materials[faceMeshIndex];
            modelMesh.subMeshCount += 1;
        }
        modelMesh.SetSubMesh(modelMesh.subMeshCount - 1, new SubMeshDescriptor(subMeshDescriptor.indexStart + MeshCutStartIndex, MeshCutEndIndex - MeshCutStartIndex));
        var sharedMaterials = meshRender.sharedMaterials;
        sharedMaterials[modelMesh.subMeshCount - 1] = mouthMat;
    }

    [Button]
    private void TestSetMouth(int x, int y)
    {
        // var materials = meshRender.materials;
        // var lastMaterial = materials[^1];
        //
        // mouthMat = new Material(mouthMat);
        var pixels = mouthTx.GetPixels(128 * x, 128 * (7 - y), 128, 128);
        var texture = new Texture2D(128, 128);
        texture.SetPixels(pixels);
        texture.Apply(); 
        mouthMat.mainTexture = texture;
        // meshRender.materials[^1] = mouthMat;
        //
        // lastMaterial.mainTexture = texture;
        // materials[^1] = lastMaterial;  
        // meshRender.materials = materials;
    }

    [Button, Fold("Mesh Cut")]
    private void MoveMeshForwardMouth() => MoveMeshMouth(new Vector3(0, -0.000001f, 0));

    [Button, Fold("Mesh Cut")]
    private void MoveMeshBackMouth() => MoveMeshMouth(new Vector3(0, 0.000001f, 0));
    
    private void MoveMeshMouth(Vector3 moveValue)
    {
        var vertices = modelMesh.vertices;
        var meshTriangles = modelMesh.GetTriangles(modelMesh.subMeshCount - 1);
        foreach (var triangleIndex in meshTriangles)
        {
            vertices[triangleIndex] += moveValue;
        }
        modelMesh.vertices = vertices;
        modelMesh.RecalculateBounds(); 
        modelMesh.RecalculateNormals();
    }
    private void OnDrawGizmos()
    {
        if (false == showGizmo)
            return;
        
        var subMeshDescriptor = modelMesh.GetSubMesh(faceMeshIndex);
        for (var i = subMeshDescriptor.indexStart + MeshCutStartIndex;
             i < subMeshDescriptor.indexStart + MeshCutEndIndex;
             i += 3)
        {
            var position = transform.position ; 
            var A = position + (Quaternion.AngleAxis(-90, Vector3.right) * modelMesh.vertices[modelMesh.triangles[i]] * 100 * scale);
            var B = position + (Quaternion.AngleAxis(-90, Vector3.right) * modelMesh.vertices[modelMesh.triangles[i + 1]] * 100 * scale);
            var C = position + (Quaternion.AngleAxis(-90, Vector3.right) * modelMesh.vertices[modelMesh.triangles[i + 2]] * 100 * scale);
            Debug.DrawLine(A, B);
            Debug.DrawLine(B, C);
            Debug.DrawLine(C, A);
        }
    }
}
