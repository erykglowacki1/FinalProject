using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    private float spawnPosZ = 100;
    private float startDelay = 2.0f;
    private float spawnInterval = 1.5f;
    private float minSpawnInterval = 0.3f;
    private float decreaseSpawnInterval = 0.2f;

 

    private float elapsedTime;
    private PlayerController playerControllerScript;
    private float globalSpeedIncrease = 10.0f;
    private float speedIncreaseInterval = 5.0f;
   // private float timeSinceLastSpeedIncrease = 0.0f;




    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomObstacle", startDelay, spawnInterval);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

      
        

    }

    


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        //timeSinceLastSpeedIncrease += Time.deltaTime;
        if (elapsedTime >= speedIncreaseInterval )
        {
            IncreaseObstacleSpeedGlobally();
            elapsedTime = 0.0f;
            spawnInterval = Mathf.Max(spawnInterval - decreaseSpawnInterval, minSpawnInterval);
            // Restart the invoke repeating with the updated spawn interval
            CancelInvoke("SpawnRandomObstacle");
            InvokeRepeating("SpawnRandomObstacle", 0.0f, spawnInterval);
        }


        

    }

    void SpawnRandomObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            int laneSelection = Random.Range(0, playerControllerScript.numberOfLanes);

            int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);

            float spawnPosX = (laneSelection - 1) * playerControllerScript.laneWidth;
            Vector3 spawnPos = new Vector3(spawnPosX, 0.7f, spawnPosZ);
            GameObject newObstacle = Instantiate(obstaclePrefabs[obstacleIndex], spawnPos, obstaclePrefabs[obstacleIndex].transform.rotation);
            MoveForward moveForwardScript = newObstacle.GetComponent<MoveForward>();

            if (moveForwardScript != null)
            {
                // Set the speed of the obstacle based on the current global speed
                moveForwardScript.SetObstacleSpeed(moveForwardScript.GetSpeed() + globalSpeedIncrease);
            }
        }
    }

    void IncreaseObstacleSpeedGlobally()
    {
        MoveForward[] moveForwardScripts = FindObjectsOfType<MoveForward>();

        foreach (MoveForward moveForwardScript in moveForwardScripts)
        {
            // Increase the speed of each obstacle
            moveForwardScript.IncreaseSpeed(globalSpeedIncrease);
        }
    }


}