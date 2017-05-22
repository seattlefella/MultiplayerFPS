
using UnityEngine;
using UnityEngine.UI;

public class UserAccountLobby : MonoBehaviour
{


    public Text UserNameText;

    void Start()
    {
        if (UserAccountManager.IsLoggedIn)
        {
            UserNameText.text = UserAccountManager.PlayerUsername;   
        }
    }

    public void LogOut()
    {

        if (UserAccountManager.IsLoggedIn)
        {
            UserAccountManager.instance.LogOut();
        }

    }

}
