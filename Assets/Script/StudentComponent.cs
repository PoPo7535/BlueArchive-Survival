using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentComponent : MonoBehaviour
{
    public StudentEntity student;
    public void Start()
    {
        student = GetComponent<StudentEntity>();
    }
}
