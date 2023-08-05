using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class DatabaseManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp.Create();
            // You can now interact with Firestore using FirebaseFirestore.DefaultInstance
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void AddScore(string userId, string username, int score, float timeSurvived)
    {
        DocumentReference docRef = FirebaseFirestore.DefaultInstance.Collection("leaderboard").Document();
        Dictionary<string, object> entry = new Dictionary<string, object>
    {
        { "userId", userId },
        { "username", username },
        { "score", score },
        { "timeSurvived", timeSurvived }
    };
        await docRef.SetAsync(entry);
    }
    public async void GetTopScores(int numberOfScores)
    {
        Query query = FirebaseFirestore.DefaultInstance.Collection("leaderboard")
            .OrderByDescending("score")
            .OrderByDescending("timeSurvived")
            .Limit(numberOfScores);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        List<ScoreEntry> topScores = new List<ScoreEntry>();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            ScoreEntry entry = new ScoreEntry
            {
                Id = document.Id,
                UserId = document.GetValue<string>("userId"),
                Username = document.GetValue<string>("username"),
                Score = document.GetValue<int>("score"),
                TimeSurvived = document.GetValue<float>("timeSurvived")
            };
            topScores.Add(entry);
        }

        // Now you have the topScores list, which you can use to display the leaderboard
    }
    public void UpdateUserGameData(string userId, int score, float timeSurvived)
    {
        DocumentReference userDocRef = FirebaseFirestore.DefaultInstance.Collection("Users").Document(userId);

        Dictionary<string, object> userData = new Dictionary<string, object>
    {
        { "Score", score },
        { "TimeSurvived", timeSurvived }
    };

        userDocRef.SetAsync(userData, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Updating user game data in Firestore was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Updating user game data in Firestore encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("User game data was updated in Firestore.");
        });
    }

    public class ScoreEntry
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public float TimeSurvived { get; set; }
    }
}
