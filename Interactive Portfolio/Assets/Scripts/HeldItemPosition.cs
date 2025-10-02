using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HeldItemPosition : MonoBehaviour
{
   public static HeldItemPosition _instance;

    private GameObject _heldItem;

    public Slider _powerSlider;

    public TMP_Text _infoText;

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
            if (_powerSlider.gameObject.activeSelf == false) { _powerSlider.gameObject.SetActive(true); }
            if (_throwStrength > 10) { return; }
           _throwStrength += Time.deltaTime * 3;
            _powerSlider.value = _throwStrength;
            Debug.Log("Strength Level: " + _throwStrength);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            ThrowItem();
            _powerSlider.gameObject.SetActive(false);
            _throwStrength = 2;
        }
    }

    public void PickUpItem(GameObject item) 
    {
        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        item.GetComponent<Collider>().enabled = false;
        item.transform.localPosition = transform.position;
        _infoText.gameObject.SetActive(true);
        _infoText.text = "Press Q to Drop or Hold Q to Throw";
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
        _infoText.gameObject.SetActive(false);
        _heldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _heldItem.GetComponent<Rigidbody>().AddExplosionForce(_throwStrength * 1.5f, transform.position,5,5);
        _heldItem.GetComponent<Rigidbody>().AddForce(transform.forward*_throwStrength);
        _heldItem.GetComponent<Collider>().enabled = true;
        _heldItem = null;

    }
}
