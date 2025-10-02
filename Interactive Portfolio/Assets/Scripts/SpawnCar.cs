using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    public List<GameObject> Veichles;

    public int Direction;

    public float minSpawnInterval = 1f; 
    public float maxSpawnInterval = 5f; 
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);

    public float ObjOrientation;


    private void Awake()
    {
        Veichles = new List<GameObject>(Resources.LoadAll<GameObject>("Cars"));
    }

    private void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnObjects());
    }

    public void stopLoop() 
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Wait for a random interval before spawning the next object
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            int RandNum = Random.Range(0, Veichles.Count);

            // Calculate a random position within the spawn area
            Vector3 spawnPosition = gameObject.transform.position;

            // Instantiate the object at the random position
            Instantiate(Veichles[RandNum], spawnPosition, Quaternion.Euler(0, ObjOrientation, 0)).GetComponent<CarMovment>().Direction = Direction;
        }
    }

    
}
