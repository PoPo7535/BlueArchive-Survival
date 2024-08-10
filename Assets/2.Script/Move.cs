using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Move : MonoBehaviour
{
    private Rigidbody rigi;
    public float moveSpeed = 30;
    public float rotateSpeed = 30;
    public Animation ani;
    private void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        if (x == 0 && y == 0)
        {
            rigi.velocity = new Vector3(0, rigi.velocity.y, 0);
            ani.Play("Kayoko_Original_Formation_Idle");
            return; 
        }

        ani.Play("Kayoko_Original_Move_Ing");
        var dir = new Vector2(x, y).normalized * (moveSpeed * Time.deltaTime);
        rigi.velocity = new Vector3(dir.x, rigi.velocity.y, dir.y);
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
