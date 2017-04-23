using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Text KillCount;
    public Text DeathCount;


    void Start()
    {
        if (UserAccountManager.IsLoggedIn)
        {
            UserAccountManager.instance.GetUserData(OnRecievedData);
        }

    }


    void OnRecievedData(string _data)
    {
        // data contains our own non secure format [kills]12/
        Debug.Log(_data);

        if (DeathCount != null && KillCount != null)
        {
            KillCount.text = DataTranslator.DataToKills(_data).ToString();
            DeathCount.text = DataTranslator.DataToDeaths(_data).ToString();
        }


    }
}
