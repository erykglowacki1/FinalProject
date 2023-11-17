using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private float spawnRangeX = 7;
    private float spawnPosZ = 100;
    private float startDelay = 2.0f;
    private float spawnInterval = 1.5f;
    private PlayerController playerControllerScript;

    //private bool gameOver = false;  // Assuming you have a game over state in PlayerController

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomObstacle", startDelay, spawnInterval);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            int animalIndex = Random.Range(0, obstaclePrefabs.Length);
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
            Instantiate(obstaclePrefabs[animalIndex], spawnPos, obstaclePrefabs[animalIndex].transform.rotation);
        }
    }

    
}
