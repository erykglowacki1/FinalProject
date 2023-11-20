using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public int numberOfLanes = 3;
    public float speed = 10.0f;
    public float floorWidth = 20.0f;
    public float laneWidth;

    private float currentLane = 1;
    private Rigidbody playerRb;
    public float jumpForce;
    public bool isOnGround = true;
    public float gravityModifier;
    public bool gameOver = false;
    public Texts texts;
    public Button restartButton;
    private bool isInvincible = false;
    private float powerupDuration = 5.0f;
    private float powerupTimer;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;

        // Calculate lane width based on the floor width and the number of lanes
        laneWidth = floorWidth / numberOfLanes;

    }

    // Update is called once per frame
    void Update()
    {
        // Get horizontal input for movement
        float horizontalInput = Input.GetAxis("Horizontal");

        // Switch lanes smoothly
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < numberOfLanes - 1)
        {
            currentLane++;
        }

        // Calculate the target position based on the chosen lane
        float targetX = currentLane * laneWidth - floorWidth / 2.0f + laneWidth / 2.0f;

        // Move the player to the target position smoothly
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), Time.deltaTime * speed);

        // Clamp the player's position within the road boundaries
        float clampedX = Mathf.Clamp(transform.position.x, -floorWidth / 2.0f + laneWidth / 2.0f, floorWidth / 2.0f - laneWidth / 2.0f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // Apply horizontal movement
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }


        //Invincsibilty Timer Logic
        if (isInvincible)
        {
            powerupTimer -= Time.deltaTime;
            powerupTimer = Mathf.Max(0, powerupTimer);

            texts.InvincibilityText(powerupTimer);
            if (powerupTimer <= 0)
            {
                isInvincible = false;
                texts.gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                gameOver = true;
                Debug.Log("Game Over!");
                texts.gameoverText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
            }
        }


        if (collision.gameObject.CompareTag("Coin"))
        {
            CollectCoin(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PowerUp"))
        {
            CollectPowerup(collision.gameObject);
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CollectCoin(GameObject Coin)
    {
        texts.score += 100;
        Destroy(Coin);
    }

    
    private void CollectPowerup(GameObject PowerUp)
    {
        StartCoroutine(ActivateInvincibility());
        powerupTimer = powerupDuration;
        texts.InvincibilityText(powerupTimer);
       
        Destroy(PowerUp);
    }

    //works with Powerup
    IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        // Wait for 5 seconds
        yield return new WaitForSeconds(5.0f);

        // End invincibility after 5 seconds
        isInvincible = false;
    }


}
