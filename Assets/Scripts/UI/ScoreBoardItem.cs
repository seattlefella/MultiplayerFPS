using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardItem : MonoBehaviour
{


    [SerializeField]
    private Text userNameText;
    [SerializeField]
    private Text killsText;
    [SerializeField]
    private Text deathsText;


    public void Setup(string _userName, int _kills, int _deaths)
    {
        userNameText.text = _userName;
        killsText.text = "Kills: " + _kills;
        deathsText.text = "Deaths: " + _deaths;

    }
}
