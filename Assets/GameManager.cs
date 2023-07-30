using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Reference to your Main Camera in the game scene
    public Camera mainCamera;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void EndGame()
    {
        // This is where you would handle any other game over logic, such as calculating the player's final score

        // Destroy the Main Camera before changing scenes
        if (mainCamera != null)
        {
            Destroy(mainCamera.gameObject);
        }

        // Then load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
