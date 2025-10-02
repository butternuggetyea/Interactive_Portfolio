using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopLights : MonoBehaviour
{
    public Light blueLight;
    public Light redLight;

    private void Awake()
    {
        InvokeRepeating("BlinkLights", 0, 1.25f);
    }

    private void BlinkLights()
    {
        if (blueLight.enabled)
        {
            blueLight.enabled = false;
            redLight.enabled = true;
        }
        else
        {
            blueLight.enabled = true;
            redLight.enabled = false;
        }
    }

}


