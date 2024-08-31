using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform tr;

    public Vector3 off;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = GameManager.I.player.transform.position + off;
    }
}
