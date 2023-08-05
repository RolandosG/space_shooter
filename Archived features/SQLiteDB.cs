using UnityEngine;
using SQLite;
using System.Collections.Generic;

public class SQLiteDB : MonoBehaviour
{
    private string connectionString;
    private SQLiteConnection dbConnection;

    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/Spaceshoot.db";
        OpenConnection();
        CreateTable();
        ReadDataFromDatabase();
        CloseConnection();
    }

    private void OpenConnection()
    {
        dbConnection = new SQLiteConnection(connectionString);
    }

    private void ReadDataFromDatabase()
    {
        var command = dbConnection.CreateCommand("SELECT * FROM YourTableName");
        var result = command.ExecuteQuery<SpaceShootClass>();

        foreach (var item in result)
        {
            Debug.Log("ID: " + item.Id + ", Name: " + item.Name);
        }
    }

    private void CloseConnection()
    {
        dbConnection.Close();
    }
    private void CreateTable()
    {
        dbConnection.CreateTable<ScoreEntry>();
    }
    public void AddScore(int score, float timeSurvived)
    {
        ScoreEntry newEntry = new ScoreEntry
        {
            Score = score,
            TimeSurvived = timeSurvived
        };

        dbConnection.Insert(newEntry);
    }
    public List<ScoreEntry> GetTopScores(int numberOfScores)
    {
        return dbConnection.Table<ScoreEntry>()
            .OrderByDescending(entry => entry.Score)
            .ThenByDescending(entry => entry.TimeSurvived)
            .Take(numberOfScores)
            .ToList();
    }
}

[System.Serializable]
public class SpaceShootClass
{
    public int Id { get; set; }
    public string Name { get; set; }
}
[System.Serializable]
public class ScoreEntry
{
    public int Id { get; set; }
    public int Score { get; set; }
    public float TimeSurvived { get; set; }
}

