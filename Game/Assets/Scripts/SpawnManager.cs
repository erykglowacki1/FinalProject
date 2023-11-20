using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    //private float spawnRangeX = 7;
    private float spawnPosZ = 100;
    private float startDelay = 2.0f;
    private float spawnInterval = 1.5f;
    private float minSpawnInterval = 0.3f;
    private float decreaseSpawnInterval = 1.0f;
    
    private PlayerController playerControllerScript;
    
    

    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomObstacle", startDelay, spawnInterval);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(UpdateSpawnInterval());
        
    }

    IEnumerator UpdateSpawnInterval()
    {
        while (spawnInterval > minSpawnInterval)
        {
            yield return  new WaitForSeconds(3);
            spawnInterval = Mathf.Max(spawnInterval -= decreaseSpawnInterval, minSpawnInterval);
            
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            int laneSelection = Random.Range(0, playerControllerScript.numberOfLanes);

            int animalIndex = Random.Range(0, obstaclePrefabs.Length);
            
            float spawnPosX = (laneSelection - 1) * playerControllerScript.laneWidth;
            Vector3 spawnPos = new Vector3(spawnPosX, 1.5f, spawnPosZ);
            Instantiate(obstaclePrefabs[animalIndex], spawnPos, obstaclePrefabs[animalIndex].transform.rotation);



        }
    }

    
}
