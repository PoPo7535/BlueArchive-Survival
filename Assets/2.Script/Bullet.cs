using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime),Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"Enemy"))
        {
            other.GetComponent<Monster>().Damage();
            gameObject.SetActive(false);
        }
    }
}
