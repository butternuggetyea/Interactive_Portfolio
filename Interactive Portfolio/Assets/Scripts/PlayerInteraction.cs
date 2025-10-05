using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerInteraction : MonoBehaviour
{
    public float _interactDistance = 10;
    public LayerMask _interactLayer;

    public GameObject interactPanel;
    public GameObject settingsPanel;
    private void Update()
    {
        GetInput();
        
    }

    private void FixedUpdate()
    {

        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInteractable, _interactDistance, _interactLayer))
        {
            interactPanel.SetActive(true);
        }
        else 
        {
            interactPanel.SetActive(false);
        }
    }

    private void GetInput() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            CastRay();
        }
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
            if (settingsPanel.activeSelf) 
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            
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
