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
    public Button returnMenuButton;
    private bool isInvincible = false;
    private float powerupDuration = 5.0f;
    private float powerupTimer;

   

    private bool doubleJumpAvailable = true;
    private float doubleJumpDuration = 5.0f; 
    private float doubleJumpTimer = 0.0f;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround || (doubleJumpAvailable && doubleJumpTimer > 0))
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z); // Reset vertical velocity

                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                if (!isOnGround)
                {
                    doubleJumpAvailable = false;
                    doubleJumpTimer = 0.0f; // Reset timer when double jumping
                }

                isOnGround = false;
            }
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
                texts.invincibilityText.gameObject.SetActive(false);
                gameOver = false;
            }
        }

        if (!isOnGround && doubleJumpTimer > 0)
        {
            doubleJumpTimer -= Time.deltaTime;

            if (doubleJumpTimer <= 0)
            {
                doubleJumpAvailable = false;
                texts.doubleJumpPowerupText.gameObject.SetActive(false);
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
            if (!isInvincible && !gameOver)
            {
                gameOver = true;
                texts.gameoverText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                returnMenuButton.gameObject.SetActive(true);
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
        else if(collision.gameObject.CompareTag("Powerup2"))
        {
            CollectDoubleJump(collision.gameObject);
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReturnMenu()
    {
        SceneManager.LoadSceneAsync(0);
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


    private void CollectDoubleJump(GameObject PowerUp)
    {
        StartCoroutine(ActivateDoubleJump());
        Destroy(PowerUp);
        texts.doubleJumpText(doubleJumpDuration);
    }

    IEnumerator ActivateDoubleJump()
    {
        doubleJumpAvailable = true;
        doubleJumpTimer = doubleJumpDuration;

        yield return new WaitForSeconds(doubleJumpDuration);

        doubleJumpAvailable = false;
    }

}