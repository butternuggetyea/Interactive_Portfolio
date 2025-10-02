using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeCar : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") || other.CompareTag("Police")) 
        {
            Destroy(other.gameObject);
        }
    

    }
}
