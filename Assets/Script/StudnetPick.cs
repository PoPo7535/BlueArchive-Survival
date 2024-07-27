using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class StudnetMouseInteract : StudentComponent
{
    
    public Outline outline;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) // (0: 왼쪽, 1: 오른쪽, 2: 가운데)
        {
            Debug.Log("왼");
        }
        
        if (Input.GetMouseButtonDown(1)) // (0: 왼쪽, 1: 오른쪽, 2: 가운데)
        {
            Debug.Log("오");
        }
    }
    

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.RaycastAll(ray, 100f);
        
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].collider.CompareTag($"Ground"))
            {
                var cameraPoint = Camera.main.transform.position;
                cameraPoint.y = hit[i].point.y;
                var cameraDir = cameraPoint - hit[i].point;

                var h = 0.5f; // 높이
                var angleB = 180-Vector3.Angle(ray.direction, cameraDir); // 각도 B (90도가 아닌 다른 각도)
                var angleBInRadians = angleB * Mathf.PI / 180.0f;
                
                var c = h / Mathf.Sin(angleBInRadians);
                var hitPoint = hit[i].point;
                gameObject.transform.position = hitPoint - ray.direction * c;
            }
        }
    }

    private void OnMouseUp()
    {
    }

    private void OnMouseEnter() => outline.enabled = true;
    private void OnMouseExit() => outline.enabled = false;
}
