using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

}
