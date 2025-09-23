using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    float _interactDistance = 10;
    public LayerMask _interactLayer;

    private void Update()
    {
        
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

        }

    }

}
