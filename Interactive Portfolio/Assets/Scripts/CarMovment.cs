using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class CarMovment : MonoBehaviour
{


    Rigidbody rb;

    public float moveSpeed = 7;

    public float maxVelocity = 10f;

    public int Direction;


    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    public GameObject wheel4;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
       

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {

            PlayerMovement.animator.SetBool("Falling", true);
            collision.collider.enabled = false;
            Invoke("dothing", .5f);

        }
    }

    private void dothing() { LoadCheckpoint.self.Reload(); PlayerMovement.animator.SetBool("Falling", false); }

    void Start()
    {

        if (Direction == 0)
        {
            GetComponentInChildren<CarBobble>().bounceXOffset -= (GetComponentInChildren<CarBobble>().bounceXOffset * 2);
        }
        if (Direction == 1)
        {
            InvokeRepeating("MoveTowards", 0, .25f);
        }

        else 
        {
            InvokeRepeating("MoveAway", 0, .25f);
        }
    }

    private void Update()
    {
        SpinWheels();
    }

    private void SpinWheels() 
    {
        wheel1.transform.Rotate(-3, 0, 0);
        wheel2.transform.Rotate(-3, 0, 0);
        wheel3.transform.Rotate(-3, 0, 0);
        wheel4.transform.Rotate(-3, 0, 0);
    }

    private void MoveTowards() 
    {
        rb.AddForce(new Vector3(moveSpeed, 0, 0), ForceMode.VelocityChange);
        CapVelocity();
    }

    private void MoveAway()
    {
        rb.AddForce(new Vector3(-moveSpeed, 0, 0), ForceMode.VelocityChange);
        CapVelocity();
    }


    void CapVelocity()
    {
        // Get the current velocity
        Vector3 velocity = rb.linearVelocity;

        // If the velocity exceeds the maximum allowed speed, clamp it
        if (velocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = velocity.normalized * maxVelocity;
        }
    }
}
