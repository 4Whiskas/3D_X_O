using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : MonoBehaviour
{
    public GameObject rotator;

    public float whereRotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate()
    {
        rotator.transform.Rotate(0, whereRotate * Time.deltaTime, 0);
    }
}
