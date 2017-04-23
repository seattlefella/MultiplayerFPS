using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DatabaseControl;

public class UserAccountManager : MonoBehaviour
{

    public static UserAccountManager instance;
    //These store the username and password of the player when they have logged in
    public static string PlayerUsername { get; protected set; }

    private static string playerPassword = "";
    public static bool IsLoggedIn { get; protected set; }

    public static string    LoggedInData { get; protected set; }

    public string LoggedInSceneName = "Lobby";
    public string LoggedOutSceneName = "LoginMenu";

    public delegate void OnDataReceivedCallBack(string data);

    void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void LogOut()
    {
        PlayerUsername = "";
        playerPassword = "";

        IsLoggedIn = false;
        Debug.Log("We have logged our player out");
        SceneManager.LoadScene(LoggedOutSceneName);
    }

    public void LogIn(string _username, string _password)
    {
        PlayerUsername = _username;
        playerPassword = _password;

        IsLoggedIn = true;

        Debug.Log("We have logged our player: " + PlayerUsername + " in");
        SceneManager.LoadScene(LoggedInSceneName);

    }

    public void SendData(string _data)
    {
        //Called when the player hits 'Set Data' to change the data string on their account. Switches UI to 'Loading...' and starts coroutine to set the players data string on the server

        if (IsLoggedIn)
        {
            StartCoroutine(SetData(_data));
        }

    }

    IEnumerator SetData(string data)
    {
        IEnumerator e = DCF.SetUserData(PlayerUsername, playerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //The data string was set correctly. Goes back to LoggedIn UI
        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            PlayerUsername = "";
            playerPassword = "";

        }
    }

    public void GetUserData(OnDataReceivedCallBack _onDataRecieved)
    {
        //Called when the player hits 'Get Data' to retrieve the data string on their account. Switches UI to 'Loading...' and starts coroutine to get the players data string from the server

        if (IsLoggedIn && UserAccountManager.instance != null)
        {
            StartCoroutine(GetData( _onDataRecieved));           
        }

    }

    IEnumerator GetData(OnDataReceivedCallBack _onDataRecieved)
    {
        IEnumerator e = DCF.GetUserData(PlayerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            PlayerUsername = "";
            playerPassword = "";
        }
        else
        {
            //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
            if (response != null)
            {
                _onDataRecieved.Invoke(response);

            }
        }
    }

}
