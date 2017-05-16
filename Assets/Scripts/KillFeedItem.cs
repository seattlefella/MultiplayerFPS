using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{

    [SerializeField] private Text text;

    public void Setup(string _userName, string _killer)
    {
        text.text = "<b>" + _userName + "</b>" + "<color=red> Killed </color>" +  "<b>" + _killer + "</b>";
    }

}
