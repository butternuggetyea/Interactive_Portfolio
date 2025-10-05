using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMouseSensitivity : MonoBehaviour
{

    public Slider _slider;
    public TMP_Text _text;
  

    public void UpdateSensitvity() 
    {
        PlayerCameraMovement.playerCamera.SetMouseSensitivity( _slider.value );
        _text.text = _slider.value.ToString();
    }
}
