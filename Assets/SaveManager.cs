using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private PlayerData playerData;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Load();
    }
    private void Start()
    {
        // Load player data when the game starts
        playerData = Load();
        // Do something with playerData...
    }

    // Save function
    public void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
        Debug.Log("Data saved: " + json);
    }

    // Load function
    public PlayerData Load()
    {
        PlayerData data;

        if (PlayerPrefs.HasKey("SaveData"))
        {
            string json = PlayerPrefs.GetString("SaveData");
            data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Data loaded: " + json);
        }
        else
        {
            data = new PlayerData();

            // Initialize your player data with correct default values:
            data.scoreMultiplierLevel = 1;  // or whatever the default level should be

            Save(data);
            Debug.Log("No saved data found, creating new PlayerData");
        }

        return data;
    }
}

[System.Serializable]
public class PlayerData
{
    public int damage; // field for damage level

    public float health;
    public float speed;
    public float score;
    public float currency;
    public int healthUpgradeLevel;
    public int fireRateUpgradeLevel;
    public int scoreMultiplierLevel;
    public float longestTimeSurvived;

    public bool hasDashAbility = false; // Set default value to false

    public SkillList skills;
}
[System.Serializable]
public struct SkillList
{
    public bool hasDashSkill;
    public int fireRateLevel;
    public int healthUpgradeLevel;
    public int speedUpgradeLevel;
    // add other skills as needed
}