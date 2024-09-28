using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static List<Transform> GetAllChild(this Transform transform)
    {
        Queue<Transform> queue = new();
        List<Transform> list = new();
        queue.Enqueue(transform);
        
        while (0 != queue.Count)
        {
            var tr = queue.Dequeue();
            list.Add(tr);
            for (int i = 0; i<tr.childCount; ++i)
            {
                queue.Enqueue(tr.GetChild(i));
            }
        }
        return list;
    }
    public static void SetActive(this CanvasGroup cg, bool isActive)
    {
        cg.interactable = isActive;
        cg.blocksRaycasts = isActive;
        cg.alpha = isActive ? 1 : 0;
    }
}
