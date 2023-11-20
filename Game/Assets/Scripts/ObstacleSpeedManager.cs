using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpeedManager : MonoBehaviour
{
    public float speed = 30.0f;
    private float increaseBySpeed = 50.0f;
    private float timeInterval = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateSpeed());
    }
    IEnumerator UpdateSpeed()
    {


        while (true)
        {
            yield return new WaitForSeconds(timeInterval);
            speed += increaseBySpeed;
            Debug.Log("Speed increased to: " + speed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
