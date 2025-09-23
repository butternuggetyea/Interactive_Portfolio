using UnityEngine;
using static UnityEditor.Progress;

public class HeldItemPosition : MonoBehaviour
{
   public static HeldItemPosition _instance;

    private GameObject _heldItem;

    private void Awake()
    {
        _instance = this;
    }

    public void PickUpItem(GameObject item) 
    {
        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        item.GetComponent<Collider>().enabled = false;
        item.transform.position = transform.position;
        
        _heldItem = item; 
    }
    public void DropItem() 
    {
        _heldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _heldItem.GetComponent<Collider>().enabled = true;
        _heldItem = null;
    }
}
