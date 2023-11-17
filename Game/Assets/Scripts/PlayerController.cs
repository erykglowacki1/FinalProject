using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float laneWidth;
    public float currentLane;

    public int numberOfLanes;
    public float speed = 10.0f;
    public float xRange = 10.0f;
    private Rigidbody playerRb;
    public float jumpForce;
    public bool isOnGround = true;
    public float gravityModifier;
    public bool gameOver = false;
    public Texts texts;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
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
        float targetX = currentLane * laneWidth - xRange / 2.0f;

        // Move the player to the target position smoothly
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), Time.deltaTime * speed);

        // Clamp the player's position within the road boundaries
        float clampedX = Mathf.Clamp(transform.position.x, -xRange / 2.0f, xRange / 2.0f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // Apply horizontal movement
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
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
            gameOver = true;
            Debug.Log("Game Over!");
            texts.gameoverText.gameObject.SetActive(true);
        }
    }
}
