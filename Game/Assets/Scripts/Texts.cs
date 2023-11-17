using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Texts : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameoverText;
    
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        StartCoroutine(IncreaseScoreCoroutine());
       
    }

    IEnumerator IncreaseScoreCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            UpdateScore(5);
        }
    }

    private void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collision with Enemy detected");
            GameOver();
        }
    }

    public void GameOver()
    {
        gameoverText.gameObject.SetActive(true);
        // You might want to add additional game over logic here.
    }
}