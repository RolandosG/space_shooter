using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text scoreText;
    public Text currencyText;
    private PlayerData loadedData;

    private void Start()
    {
        Debug.Log("MainMenu.Start");

        if (scoreText == null)
        {
            Debug.Log("scoreText is null");
        }
        else
        {
            Debug.Log("scoreText is not null");
        }

        loadedData = SaveManager.Instance.Load();

        if (loadedData == null)
        {
            Debug.Log("loadedData is null");
        }
        else
        {
            Debug.Log("loadedData is not null, score: " + loadedData.score);
            scoreText.text = "Score: " + loadedData.score;
            currencyText.text = "$: " + loadedData.currency;

        }
    }

    public void ConvertScoreToCurrency()
    {
        float conversionRate = 10f;  // Adjust this to change how many points make up 1 unit of currency
        float currencyToAdd = loadedData.score / conversionRate;

        // Subtract the equivalent score
        loadedData.score -= currencyToAdd * conversionRate;

        // Add to the currency
        loadedData.currency += currencyToAdd;

        // Save the new data
        SaveManager.Instance.Save(loadedData);

        // Update the UI
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (loadedData != null) // Check that some data was actually loaded
        {
            scoreText.text = "Score: " + loadedData.score;
            currencyText.text = "$: " + loadedData.currency;
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
