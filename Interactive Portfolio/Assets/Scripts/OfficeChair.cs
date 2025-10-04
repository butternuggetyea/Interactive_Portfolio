using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficeChair : InteractableObject
{

    public TMP_Text _infoText;

    public GameObject _slider;
    public GameObject _Retical;

    public override void Interact()
    {
        SitDown();
    }

    GameObject _player;
    public Transform _exitPos;
    private void SitDown() 
    {
        _infoText.gameObject.SetActive(true);
        _infoText.text = "Press Q to stand up";
        SetUi();
      _player = PlayerMovement.animator.gameObject;
        PlayerCameraMovement.playerCamera.DisableCamera();
        PlayerMovement.playerMovement.DisableMovement();
        _player.transform.position = transform.position;
      _player.transform.rotation = transform.rotation;


    }

    private void SetUi() 
    {
        if (_Retical.gameObject.activeSelf)
        {
            _Retical.SetActive(false);
            _slider.SetActive(true);

        }
        else 
        {
            _Retical.SetActive(true);
            _slider.SetActive(false);
        }
    }

    private void Update()
    {
        StandUp();
    }

    public void StandUp() 
    {
        if (_player == null) { return; }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetUi();
            _infoText.gameObject.SetActive(false);
            PlayerMovement.playerMovement.EnableMovement();
            PlayerCameraMovement.playerCamera.EnableCamera();
            _player.transform.position = _exitPos.position;
            
            _player = null;
        }
    }

}
