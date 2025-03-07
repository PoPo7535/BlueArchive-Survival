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
    private bool IndexError(int value) => value % 3 == 0;
    [ValidateInput("IndexError", "startIndex값은 3의 배수여야 합니다")]
    [Fold("Mesh Cut")]
    public int meshCutStartIndex = 0;
    
    [ValidateInput("IndexError", "endIndex값은 3의 배수여야 합니다")]
    [Fold("Mesh Cut")]
    public int meshCutEndIndex = 96;

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
        if (meshCutStartIndex == 0)
            modelMesh.SetSubMesh(faceMeshIndex, new SubMeshDescriptor(subMeshDescriptor.indexStart + meshCutEndIndex, subMeshDescriptor.indexCount - meshCutEndIndex));
        else
        {
            var subMesh1 = new SubMeshDescriptor(subMeshDescriptor.indexStart, meshCutStartIndex);
            var subMesh2 = new SubMeshDescriptor(subMeshDescriptor.indexStart + meshCutEndIndex, subMeshDescriptor.indexCount - meshCutEndIndex);
            
            modelMesh.SetSubMesh(faceMeshIndex, subMesh1);
            modelMesh.SetSubMesh(modelMesh.subMeshCount - 1, subMesh2);
            meshRender.sharedMaterials[modelMesh.subMeshCount - 1] = meshRender.sharedMaterials[faceMeshIndex];
            modelMesh.subMeshCount += 1;
        }
        modelMesh.SetSubMesh(modelMesh.subMeshCount - 1, new SubMeshDescriptor(subMeshDescriptor.indexStart + meshCutStartIndex, meshCutEndIndex - meshCutStartIndex));
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
        for (var i = subMeshDescriptor.indexStart + meshCutStartIndex;
             i < subMeshDescriptor.indexStart + meshCutEndIndex;
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
