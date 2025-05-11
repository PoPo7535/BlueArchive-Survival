using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SetMat : MonoBehaviour
{

    public Material mouthMat;
    public Texture2D mouthTx;
    public SkinnedMeshRenderer meshRender;
    public bool instMat = true;    
    private void Awake()
    {
        if (instMat)
        {
            gameObject.name.Log();
            var materials = meshRender.materials;
            meshRender.materials = materials;
            mouthMat = meshRender.materials[^1];
        }

        SetMouth(0, 0);
    }

    [Button]
    private void SetMouth(int x, int y)
    {
        var pixels = mouthTx.GetPixels(128 * x, 128 * (7 - y), 128, 128);
        var texture = new Texture2D(128, 128);
        texture.SetPixels(pixels);
        texture.Apply(); 
        mouthMat.mainTexture = texture;
    }
}
