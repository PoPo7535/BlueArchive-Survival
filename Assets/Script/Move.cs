using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public CharacterController cc;
    public float speed = 50;
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        cc.Move(new Vector3(x, 0, y).normalized * speed);
        Debug.Log(new Vector3(0,Vector3.Angle(Vector3.forward, new Vector3(x, 0, y)),0));
        transform.eulerAngles = new Vector3(0,Vector3.Angle(Vector3.forward, new Vector3(x, 0, y)),0);
    }
    
}
