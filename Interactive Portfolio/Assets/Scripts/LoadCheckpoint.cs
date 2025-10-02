using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCheckpoint : MonoBehaviour
{

    public static LoadCheckpoint self;

    Rigidbody rb;

    public static Vector3 checkPointPos;

    public static event EventHandler loadCheckpoint;
    private void Awake()
    {

        rb = GetComponent<Rigidbody>();
        self = this;

        checkPointPos = transform.position;
        loadCheckpoint += LoadLastPoint;
    }

    public void Reload()
    {
         loadCheckpoint?.Invoke(this, EventArgs.Empty);
    }

    private void LoadLastPoint(object sender, EventArgs e) 
    {
        GetComponent<Collider>().enabled = true;
        rb.linearVelocity = Vector3.zero;
        transform.position = checkPointPos;
    }
}
