using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{

    public static bool IsOn = false;
    private Player[] players;
    [SerializeField] private GameObject playerScoreBoardItem;

    [SerializeField] private Transform playerScoreBoardList;


    void OnEnable()
    {

        IsOn = true;
        players = GameManager.GetAllPlayers();

        foreach (Player _player in players)
        {
            Debug.Log("Player Name: " + _player.UserName + " Kills: " + _player.Kills + " Deaths: " + _player.Deaths);
            GameObject itemGO = (GameObject) Instantiate(playerScoreBoardItem, playerScoreBoardList);

            ScoreBoardItem item = itemGO.GetComponent<ScoreBoardItem>();
            item.Setup(_player.UserName,_player.Kills,_player.Deaths);
        }


    }
    void OnDisable()
    {
        IsOn = false;

        foreach (Transform child in playerScoreBoardList)
        {
            Destroy(child.gameObject);
        }

  
    }
}
