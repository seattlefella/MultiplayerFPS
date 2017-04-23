using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour {

        private static Dictionary<string,Player> players = new Dictionary<string, Player>();
        private const string PLAYERIDPREFIX ="Player";
        private static string playerID = "TBD";

        public delegate void    OnPlayerKilledCallback(string playerKilled, string sourceOfDamage);

        public OnPlayerKilledCallback onPlayerKilledCallback;

        // the game manager will be set up as a singleton
        public static GameManager Instance;
        public MatchSettings MatchSettings;


        [SerializeField]
        private GameObject sceneCamera;


        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Some one tried to set up more than one gameManager scripts");
            }
            else
            {
                Instance = this;
            }
        }

        public void SetSceneCameraActive(bool _isActive)
        {

            if (sceneCamera == null)
            {
                return;
            }

            else
            {
                sceneCamera.SetActive(_isActive);
            }

        }


        #region Player Setup and control

        public static void RegisterPlayer(string _netID, Player _player)
        {
            playerID = PLAYERIDPREFIX + _netID;
            players.Add(playerID, _player);
            _player.transform.name = playerID;
        }

        public static void UnRegisterPlayer(string _playerID)
        {
            players.Remove(_playerID);
        }

        public static Player GetPlayer(string _playerID)
        {
            return players[_playerID];
        }

        public static Player[] GetAllPlayers()
        {
            return players.Values.ToArray();
        }

        #endregion




        #region For debug purposes, display who is playing on the screen.

        //////void OnGUI()
        //////{
        //////    GUILayout.BeginArea(new Rect(200,200,200,500));
        //////        GUILayout.BeginVertical();

        //////    foreach (string _playerID in players.Keys)
        //////    {
        //////        GUILayout.Label(_playerID + "   XXX   " + players[_playerID].transform.name);
        //////    }

        //////        GUILayout.EndVertical();
        //////    GUILayout.EndArea();
        //////}

        #endregion
    }
}
