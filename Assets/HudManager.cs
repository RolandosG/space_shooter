using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    public Text health;
    public Text score;
    private Text scoreText;
    public Canvas canvas;
    public Text waveText; // or `private Text waveText;` for the old UI

   
    private void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    public void UpdateHealthText(float hpText)
    {
        this.health.text = "Health: " + hpText;
    }
    public void UpdateScoreText(float scoreText)
    {
        this.score.text = "Score: " + scoreText;
    }
    public void UpdateWaveText(int waveNumber)
    {
        this.waveText.text = "Wave: " + waveNumber;
    }

}
