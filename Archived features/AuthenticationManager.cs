using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using System;

public class AuthenticationManager : MonoBehaviour
{
    // Registration input fields
    public TMP_InputField registrationEmailInputField;
    public TMP_InputField registrationPasswordInputField;
    public TMP_InputField registrationNicknameInputField;
    // Login input fields
    public TMP_InputField loginEmailInputField;
    public TMP_InputField loginPasswordInputField;

    //private static bool firstTimeMainMenuLoaded = true;
    public static bool IsUserLoggedIn = false;

    public GameObject preMenuPanel;
    public GameObject registrationPanel;
    public GameObject loginPanel;
    public GameObject mainMenuPanel;

    public Text registrationStatusText;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp.Create();
            // You can now interact with FirebaseAuth using FirebaseAuth.DefaultInstance

            // Check if there is a user already logged in
            FirebaseUser currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

            // Check if the user has already logged in using PlayerPrefs
            //bool userLoggedIn = PlayerPrefs.GetInt("UserLoggedIn", 0) == 1;

            // If the user is logged in and has a display name, show the main menu
            if (IsUserLoggedIn) // && currentUser != null && !string.IsNullOrEmpty(currentUser.DisplayName
            {
                // Set the current user in the UserSessionManager
                UserSessionManager.Instance.SetCurrentUser(currentUser);

                // Show the main menu panel if a user is already logged in and has a display name
                ShowMainMenuPanel();
            }
            else
            {
                // Show the pre-menu panel if no user is logged in or doesn't have a display name
                ShowPreMenuPanel();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoginUser()
    {
        string email = loginEmailInputField.text;
        string password = loginPasswordInputField.text;

        // Check if the email and password fields are empty
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email and password fields must be filled.");
            // Update the login status text or show a UI message to inform the user
            return;
        }

        Debug.Log("Signing in with email and password...");

        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("User login was canceled. Exception: " + task.Exception);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("User login encountered an error: " + task.Exception);
                AggregateException innerExceptions = task.Exception.Flatten();
                foreach (var innerException in innerExceptions.InnerExceptions)
                {
                    Debug.LogError("Inner exception: " + innerException.Message);
                }
                return;
            }
            if (task.IsCompleted)
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User login was successful: {0} ({1})", newUser.DisplayName, newUser.UserId);

                Debug.Log("Setting current user in UserSessionManager...");
                UserSessionManager.Instance.SetCurrentUser(newUser); // Store the user in UserSessionManager
                Debug.Log("Calling ShowMainMenuPanel() after successful login...");
                IsUserLoggedIn = true;
                PlayerPrefs.SetInt("UserLoggedIn", 1);
                PlayerPrefs.Save();
                ShowMainMenuPanel();
                
            }
            else
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync task did not complete as expected.");
            }
        });
        
    }

    public void RegisterUser()
    {
        string email = registrationEmailInputField.text;
        string password = registrationPasswordInputField.text;
        string nickname = registrationNicknameInputField.text;

        registrationStatusText.text = "Registering...";

        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("User registration was canceled.");
                registrationStatusText.text = "Registration canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("User registration encountered an error: " + task.Exception.GetBaseException().Message);
                registrationStatusText.text = "Error: " + task.Exception.Message;
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User registration was successful: {0} ({1})", newUser.DisplayName, newUser.UserId);
            registrationStatusText.text = "Registration successful!";
            UserSessionManager.Instance.SetCurrentUser(newUser); // Store the user in UserSessionManager
            IsUserLoggedIn = true;
            // Update the user's display name (nickname) after successful registration
            UpdateUserDisplayName(newUser, nickname);
            PlayerPrefs.SetInt("UserLoggedIn", 1);
            PlayerPrefs.Save();
            ShowMainMenuPanel();
            

        });
        
    }

    

    private void UpdateUserDisplayName(FirebaseUser user, string displayName)
    {
        UserProfile profile = new UserProfile
        {
            DisplayName = displayName
        };

        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Update user display name was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Update user display name encountered an error: " + task.Exception);
                return;
            }

            Debug.LogFormat("User display name was updated to: {0}", displayName);
        });
    }
    public void ShowRegistrationPanel()
    {
        preMenuPanel.SetActive(false);
        registrationPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void ShowLoginPanel()
    {
        preMenuPanel.SetActive(false);
        registrationPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void ShowPreMenuPanel()
    {
        preMenuPanel.SetActive(true);
        registrationPanel.SetActive(false);
        loginPanel.SetActive(false);
    }
    public void ShowMainMenuPanel()
    {
        preMenuPanel.SetActive(false);
        registrationPanel.SetActive(false);
        loginPanel.SetActive(false);

        if (mainMenuPanel == null)
        {
            Debug.LogError("mainMenuPanel is null");
        }
        else
        {
            Debug.Log("Activating mainMenuPanel...");
            mainMenuPanel.SetActive(true);
        }
    }

}
