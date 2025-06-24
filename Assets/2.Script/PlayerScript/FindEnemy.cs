using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FindEnemy
{
    public FindEnemy(Transform transform)
    {
        this.transform = transform;
    }
    public float radius = 5;
    private int frameCount = 0;
    private Transform transform;
    public GameObject nearObj
    {
        get
        {
            Find();
            return _nearObj;
        }
    }

    private GameObject _nearObj;
    public GameObject randomObj
    {
        get
        {
            Find();
            return _randomObj;
        }
    }

    private GameObject _randomObj;
    
    private void Find()
    {
        if (frameCount == Time.frameCount)
            return;
        
        frameCount = Time.frameCount;
        _nearObj = _randomObj = null;
        var colliders = Physics.OverlapSphere(transform.position, radius);
        colliders = colliders.Where(col => col.gameObject.CompareTag("Enemy")).ToArray();

        if (0 == colliders.Length)
            return;
            
        _randomObj = colliders[Random.Range(0, colliders.Length - 1)].gameObject;
        var shortDis = float.MaxValue;
        foreach (var collider in colliders)
        {
            var dis = Vector3.Distance(collider.transform.position, transform.position);
            if (dis < shortDis)
            {
                _nearObj = collider.gameObject;
                shortDis = dis;
            }
        }
    }
}
