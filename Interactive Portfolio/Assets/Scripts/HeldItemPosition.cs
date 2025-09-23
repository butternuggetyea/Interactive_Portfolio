using UnityEngine;
using static UnityEditor.Progress;

public class HeldItemPosition : MonoBehaviour
{
   public static HeldItemPosition _instance;

    private GameObject _heldItem;

    public float _throwStrength = 2;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (_heldItem == null) { return; }
        _heldItem.transform.position = transform.position;
        if (Input.GetKey(KeyCode.Q))
        {
            if (_throwStrength > 10) { return; }
           _throwStrength += Time.deltaTime * 3;
            Debug.Log("Strength Level: " + _throwStrength);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            ThrowItem();
            _throwStrength = 2;
        }
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
        if (_heldItem == null) { return; }
        _heldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _heldItem.GetComponent<Collider>().enabled = true;
        _heldItem = null;
    }

    public void ThrowItem()
    {
        if (_heldItem == null) { return; }
        _heldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _heldItem.GetComponent<Rigidbody>().AddExplosionForce(_throwStrength * 1.5f, transform.position,5,5);
        _heldItem.GetComponent<Rigidbody>().AddForce(transform.forward*_throwStrength);
        _heldItem.GetComponent<Collider>().enabled = true;
        _heldItem = null;

    }
}
