using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform tr;

    public Vector3 off;
    // Update is called once per frame
    void Update()
    {
        transform.position = tr.position + off;
    }
}
