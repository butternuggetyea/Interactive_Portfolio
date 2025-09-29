using TMPro;
using UnityEngine;

public class OfficeChair : InteractableObject
{

    public TMP_Text _infoText;

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

      _player = PlayerMovement.animator.gameObject;
        PlayerCameraMovement.playerCamera.DisableCamera();
        PlayerMovement.playerMovement.DisableMovement();
        _player.transform.position = transform.position;
      _player.transform.rotation = transform.rotation;


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
            _infoText.gameObject.SetActive(false);
            PlayerMovement.playerMovement.EnableMovement();
            PlayerCameraMovement.playerCamera.EnableCamera();
            _player.transform.position = _exitPos.position;
            
            _player = null;
        }
    }

}
