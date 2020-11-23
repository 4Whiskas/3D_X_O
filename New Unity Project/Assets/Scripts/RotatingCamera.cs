using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    public Transform rotator;
    public float speed = 5f;   
    // Start is called before the first frame update
    void Start()
    {
        rotator = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(0, speed * Time.deltaTime, 0);
    }
}
