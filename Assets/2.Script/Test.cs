using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
public class Test : MonoBehaviour
{
    public int faceMeshIndex = 2;
    
    public Mesh _skinned;
    public Vector3 offSet;
    
    private bool IndexError(int value) => value % 3 == 0;
    [ValidateInput("IndexError", "startIndex값은 3의 배수여야 합니다")]
    public int startIndex = 0;
    
    [ValidateInput("IndexError", "endIndex값은 3의 배수여야 합니다")]
    public int endIndex = 96;
    
    public Material mat;
    public Texture2D tx;

    public bool showGizmo = false;
    public bool isSet;

    [Button]
    private void Awake()
    {
        if (false == isSet)
            Mouth();
        Set(4, 0);
    }

    private void Mouth()
    {
        var subMeshDescriptor = _skinned.GetSubMesh(faceMeshIndex);
        _skinned.subMeshCount += 1;
        if (startIndex == 0)
            _skinned.SetSubMesh(faceMeshIndex, new SubMeshDescriptor(subMeshDescriptor.indexStart + endIndex, subMeshDescriptor.indexCount - endIndex));
        else
        {
            var subMesh1 = new SubMeshDescriptor(subMeshDescriptor.indexStart, startIndex);
            var subMesh2 = new SubMeshDescriptor(subMeshDescriptor.indexStart + endIndex, subMeshDescriptor.indexCount - endIndex);
            _skinned.SetSubMesh(faceMeshIndex, subMesh1);
            _skinned.SetSubMesh(_skinned.subMeshCount - 1, subMesh2);
            _skinned.subMeshCount += 1;
        }
        _skinned.SetSubMesh(_skinned.subMeshCount - 1, new SubMeshDescriptor(subMeshDescriptor.indexStart + startIndex, endIndex - startIndex));
    }

    private void Set(int x, int y)
    {
        var pixels = tx.GetPixels(128 * x, 128 * (7 - y), 128, 128);
        var texture = new Texture2D(128, 128);
        texture.SetPixels(pixels);
        texture.Apply(); 
        mat.mainTexture = texture;
    }
    private void OnDrawGizmos()
    {
        if (false == showGizmo)
            return;
        
        var subMeshDescriptor = _skinned.GetSubMesh(faceMeshIndex);
        for (var i = subMeshDescriptor.indexStart + startIndex;
             i < subMeshDescriptor.indexStart + endIndex;
             i += 3) 
        {
            var position = transform.position+offSet;
            var A = position + (Quaternion.AngleAxis(90, Vector3.right) * _skinned.vertices[_skinned.triangles[i]] * 100);
            var B = position + (Quaternion.AngleAxis(90, Vector3.right) * _skinned.vertices[_skinned.triangles[i + 1]] * 100);
            var C = position + (Quaternion.AngleAxis(90, Vector3.right) * _skinned.vertices[_skinned.triangles[i + 2]] * 100);
            Debug.DrawLine(A, B);
            Debug.DrawLine(B, C);
            Debug.DrawLine(C, A);
        }
    }
}
