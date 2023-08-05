using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;

public class UserSessionManager : MonoBehaviour
{
    public static UserSessionManager Instance;

    public FirebaseUser CurrentUser { get; private set; }
    private FirebaseUser currentUser;

    void Awake()
    {
        Debug.Log("UserSessionManager Awake()");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCurrentUser(FirebaseUser user)
    {
        Debug.Log("Setting the current user in UserSessionManager...");
        try
        {
            currentUser = user;
            Debug.Log("Current user set in UserSessionManager: " + currentUser.DisplayName + " (" + currentUser.UserId + ")");
        }
        catch (Exception e)
        {
            Debug.LogError("Error setting the current user in UserSessionManager: " + e.Message);
        }
    }
    public FirebaseUser GetCurrentUser()
    {
        Debug.Log("Getting current user in UserSessionManager...");
        return currentUser;
    }
}
