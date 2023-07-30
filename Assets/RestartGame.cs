using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using Firebase.Auth;

public class RestartGame : MonoBehaviour
{
    public GameObject gameOverScreen;

    public Text timerText;
    public Text scoreText;
    public Text finalScoreText;

    private Stats stats;
    private Timer timer;
    private float finalScore;

    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        // Stop the game time or do other game over related actions
        Time.timeScale = 1f;
        // Store the final score
        if (stats != null)
        {
            finalScore = stats.score;
        }
    }

    public void Restart()
    {
        // Restart the current scene
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;

    }
    public void MainMenu()
    {
        // Find the game camera and destroy it
        GameObject gameCamera = GameObject.FindWithTag("GameCamera");
        if (gameCamera != null)
        {
            Destroy(gameCamera);
        }

        // Then go to the Menu
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    private void OnEnable()
    {
        // Get the timer and score from the Timer and Stats components respectively
        Timer timer = FindObjectOfType<Timer>();
        Stats stats = FindObjectOfType<Stats>();
        if (timer == null)
        {
            Debug.Log("timer is null");
        }
        if (stats == null)
        {
            Debug.Log("stats is null");
        }
        if (timerText == null)
        {
            Debug.Log("timerText is null");
        }
        if (scoreText == null)
        {
            Debug.Log("scoreText is null");
        }
        if (gameOverScreen.activeInHierarchy) // Check if the Game Over screen is active
        {
            // Set the timer and score text on the retry screen
            if (timer != null)
            {
                timerText.text = "Time: " + timer.FormattedTime;
            }
            if (stats != null)
            {
                scoreText.text = "Score: " + stats.score.ToString();
                // Retrieve the stored finalScore using PlayerPrefs
                float finalScore = PlayerPrefs.GetFloat("FinalScore", 0);
                finalScoreText.text = "Final Score: " + finalScore.ToString();

            }
        }
    }
}
