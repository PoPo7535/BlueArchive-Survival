using System;
using System.Linq;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

public class FindEnemy : MonoBehaviour
{
    private float time = 0;
    private const float findTime = 0.1f;
    public float radius = 5;

    public GameObject nearObj;
    public GameObject randomObj;
    void Update()
    {
        time += Time.deltaTime;
        if (false == findTime < time) 
            return;
        
        time = 0;
        var colliders = Physics.OverlapSphere(transform.position, radius);
        colliders = colliders.Where(col => col.gameObject.CompareTag("Enemy")).ToArray();
        if (0 == colliders.Length)
        {
            nearObj = randomObj = null;
            return;
        }
            
        randomObj = colliders[Random.Range(0, colliders.Length - 1)].gameObject;
        var shortDis = float.MaxValue;
        foreach (var collider in colliders)
        {
            var dis = Vector3.Distance(collider.transform.position, transform.position);
            if (dis < shortDis)
            {
                nearObj = collider.gameObject;
                shortDis = dis;
            }
        }
    }
}
