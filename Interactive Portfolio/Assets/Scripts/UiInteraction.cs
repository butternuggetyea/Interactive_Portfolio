using UnityEngine;

public class UiInteraction : MonoBehaviour
{
    Camera cam;
    public float _interactDistance = 10;
    public LayerMask _interactLayer;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (cam.enabled && Input.GetMouseButtonDown(0)) 
        {
            CastUIRay();
            
        }
    }

    private void CastUIRay() 
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * _interactDistance, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance, _interactLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * _interactDistance, Color.blue, 1f);

            var perkDisplay = hit.transform.GetComponent<ButtonUI>();
            if (perkDisplay != null)
            {
                Debug.Log("Hit Ui");
                perkDisplay.OnClick();
            }

        }
    }

}
