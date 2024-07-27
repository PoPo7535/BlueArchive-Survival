using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Test : MonoBehaviour
{
    public int faceMeshIndex = 2;
    public int subMeshMaxCount = 5;
    
    public Mesh _skinned;
    public int startIndex = 0;
    public Vector3 offSet;
    public int endIndex = 96;
    
    
    public Material mat;
    public Texture2D tx;

    void Awake()
    {
        Mouth();
        Set(4, 0);
    }

    private void Mouth()
    {
        var subMeshDescriptor = _skinned.GetSubMesh(faceMeshIndex);
        if (subMeshMaxCount != _skinned.subMeshCount)
            return;
        BigInteger asd;
        BigInteger asd2;
        var q = asd* asd2;
        
        _skinned.subMeshCount = subMeshMaxCount+1;
        _skinned.SetSubMesh(faceMeshIndex, new SubMeshDescriptor(subMeshDescriptor.indexStart + endIndex, subMeshDescriptor.indexCount));
        _skinned.SetSubMesh(subMeshMaxCount, new SubMeshDescriptor(subMeshDescriptor.indexStart, endIndex));
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
        return;
        var subMeshDescriptor = _skinned.GetSubMesh(faceMeshIndex);
        for (int i = subMeshDescriptor.indexStart + startIndex ;
             i < subMeshDescriptor.indexStart + subMeshDescriptor.indexCount;
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
