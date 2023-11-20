using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed;
    
    private PlayerController playerControllerScript;
    

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        //speed = GameObject.Find("ObstacleManager").GetComponent<ObstacleSpeedManager>().speed;
        SetObstacleSpeed(speed);
    }

   

    
    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

        }
    
    }

    public void SetObstacleSpeed(float initialSpeed)
    {
        speed = initialSpeed;
    }

    public void IncreaseSpeed(float speedIncrease)
    {
        speed += speedIncrease;
    }
    public float GetSpeed() {
        return speed;
    }
}
