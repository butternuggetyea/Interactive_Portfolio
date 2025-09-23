using UnityEngine;

public class OfficeChair : InteractableObject
{
    public override void Interact()
    {
        SitDown();
    }

    GameObject _player;
    public Transform _exitPos;
    private void SitDown() 
    {
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
            PlayerMovement.playerMovement.EnableMovement();
            PlayerCameraMovement.playerCamera.EnableCamera();
            _player.transform.position = _exitPos.position;
            
            _player = null;
        }
    }

}
