using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBobble : MonoBehaviour
{
    public float bounceSpeed = 3.0f; // Speed of the bounce
    public float bounceHeight = .25f; // Height of the bounce
    public float bounceDuration = 2.0f; // Duration of the bounce

    public float bounceXOffset;
    public float bounceYOffset = 1.3f;
    public float bounceZOffset;

    private Vector3 originalPosition;
    private float bounceTimer = 0.0f;



    public float shakeSpeed = 1.0f; // Speed of the shake
    public float shakeIntensity = 10.0f; // Intensity of the shake
    public float shakeDuration = 2.0f; // Duration of the shake

    private Quaternion originalRotation;
    private float shakeTimer = 0.0f;


    private void Awake()
    {
        InvokeRepeating("StartShake", 0, 2);

        InvokeRepeating("StartBounce", 0, 1.5f);
    }

    public void StartShake()
    {
        shakeTimer = shakeDuration;
    }

    void Start()
    {
        originalRotation = transform.rotation;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (bounceTimer > 0)
        {
            // Calculate the bounce height based on a sine wave
            float bounceOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

            // Apply the bounce on the Y-axis
            transform.position = new Vector3 (transform.parent.position.x + bounceXOffset, transform.parent.position.y+ bounceYOffset, transform.parent.position.z + bounceZOffset) + new Vector3(0, bounceOffset, 0);

            // Decrease the timer
            bounceTimer -= Time.deltaTime;
        }
       

        if (shakeTimer > 0)
        {
            // Calculate the shake angle based on a sine wave
            float shakeAngle = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;

            // Apply the rotation on the Z-axis
            transform.rotation = originalRotation * Quaternion.Euler(0, 0, shakeAngle);

            // Decrease the timer
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Reset to the original rotation when the shake is done
            transform.rotation = originalRotation;
        }

    }

    public void StartBounce()
    {
        bounceTimer = bounceDuration;
    }
}
