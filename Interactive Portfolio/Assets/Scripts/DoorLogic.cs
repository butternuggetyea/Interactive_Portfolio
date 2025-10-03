using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : InteractableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created\

    bool _openDoor;
    bool _midState;
    private void Awake()
    {
        _openDoor = false;
    }

    public override void Interact()
    {
        SetDoorState();
    }

    private void SetDoorState() 
    {
        if (_midState == false)
        {


            if (_openDoor)
            {
                StopAllCoroutines();

                StartCoroutine(CloseDoor());
                _midState = true;
            }
            else
            {
                StopAllCoroutines();

                StartCoroutine(OpenDoor());
                _midState = true;
            }
        }
    }

    private IEnumerator OpenDoor()
    {

        for (int i = 0; i < 30; i++)
        {
            Debug.Log("OpenDoor");
            gameObject.transform.Rotate(0, -3, 0);
            yield return new WaitForSecondsRealtime(.0120f);
        }
        _openDoor = true;
        _midState = false;
    }

    private IEnumerator CloseDoor()
    {
        for (int i = 0; i < 30; i++)
        {
            Debug.Log("CloseDoor");
            gameObject.transform.Rotate(0, 3, 0);
            yield return new WaitForSecondsRealtime(.0120f);
        }
        _openDoor = false;
        _midState = false;
    }

}
