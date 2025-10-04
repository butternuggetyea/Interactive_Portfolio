using UnityEngine;
using UnityEngine.UI;

public class UpdateShapeCount : MonoBehaviour
{

    Slider _slider;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        CubeSpawner._cubeCount = (int)_slider.value;
    }

    public void SetShapeCount() 
    {
        CubeSpawner._cubeCount = (int)_slider.value;
    }
}
