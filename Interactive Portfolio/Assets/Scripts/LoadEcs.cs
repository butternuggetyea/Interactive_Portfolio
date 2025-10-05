using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadEcs : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void Awake()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
