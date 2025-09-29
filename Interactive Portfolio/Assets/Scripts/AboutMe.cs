using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutMe : MonoBehaviour
{
    public GameObject panel;
    Button button;

    public static List<GameObject> _panels = new List<GameObject>();

    private void Awake()
    {
        _panels.Add(panel);
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
            foreach (GameObject panel in _panels) 
            {
                panel.SetActive(false);
            }
            panel.SetActive(true);
        }
    }



}
