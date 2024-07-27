using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class Hexagon : MonoBehaviour
{
    public int xCount = 8;
    private float zOff = 0.862f;
    private float xOff = 1f;
    public LayerMask layerMask;
    [Button]
    public void Set()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var tr= transform.GetChild(i);
            var xPos = (i % xCount) * xOff;
            var zPos = i / xCount * zOff;
            tr.localPosition = new Vector3(xPos + (i / xCount % 2 * 0.5f), 0, zPos);
        }
    }

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.RaycastAll(ray, 100f);
        
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].collider.tag=="Field")
            {
                break;
            }
        }
    }
}
