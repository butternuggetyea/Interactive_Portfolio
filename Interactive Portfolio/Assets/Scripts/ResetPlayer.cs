using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    Collider tmp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.enabled = false;
            tmp = other;
            Invoke("dothing", 2);
        }
    }

    private void dothing() { tmp.enabled = true; LoadCheckpoint.self.Reload();  }


}
