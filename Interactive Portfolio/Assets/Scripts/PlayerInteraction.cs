using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float _interactDistance = 10;
    public LayerMask _interactLayer;

    public GameObject interactPanel;

    private void Update()
    {
        GetInput();
        
    }

    private void GetInput() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            CastRay();
        }
       
    }

    private void CastRay() 
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInteractable, _interactDistance, _interactLayer)) 
        {
            hitInteractable.transform.GetComponent<InteractableObject>().Interact();
            Debug.Log("Hit Ray");

        }


    }

}
