using UnityEngine;

public class DropperSpawner : MonoBehaviour
{
    public GameObject _dropperPos;

    private Vector2 _positionConstraints;

    private void Awake()
    {
        _positionConstraints = new Vector2(-10 , 3.25f );//min, max
    }

    private void Update()
    {
        GetInput();
    }


    private void GetInput() 
    {
        Vector3 pos = _dropperPos.transform.position;

        if (Input.GetKey(KeyCode.A)) 
        {
            if (_dropperPos.transform.position.x > _positionConstraints.x) 
            {
                _dropperPos.transform.position = new Vector3(pos.x - (Time.deltaTime*2), pos.y, pos.z);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (_dropperPos.transform.position.x < _positionConstraints.y)
            {
                Debug.Log("Pos" + _dropperPos.transform.position.x + " Con" + _positionConstraints.y);
                _dropperPos.transform.position = new Vector3(pos.x + (Time.deltaTime*2), pos.y, pos.z);
            }
        }
    }

}
