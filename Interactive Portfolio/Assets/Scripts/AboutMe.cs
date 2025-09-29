using UnityEngine;
using UnityEngine.UI;

public class AboutMe : MonoBehaviour
{
    public GameObject panel;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(DisplayInfo);
    }


    public void DisplayInfo() 
    {
        if (panel.activeSelf == true)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }



}
