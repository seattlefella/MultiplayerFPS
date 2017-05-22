using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using System;
using System.Runtime.Remoting.Messaging;

namespace Assets.Scripts
{
    public class JoinGame : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject>    roomList =   new List<GameObject>();

        [SerializeField]
        private List<MatchInfoSnapshot> currentMatchData = new List<MatchInfoSnapshot>();
         
        [SerializeField]
        private Text status;

        [SerializeField]
        private GameObject listItemPrefab;

        [SerializeField]
        private Transform roomListParent;

        private NetworkManager networkManager;

        void Start()
        {
            networkManager = NetworkManager.singleton;
            if (networkManager.matchMaker == null)
            {
                networkManager.StartMatchMaker();
            }

            // Refresh the list of rooms
            RefreshRoomList();
        }

        public void RefreshRoomList()
        {

            ClearRoomList();

            if (networkManager.matchMaker == null)
            {
                networkManager.StartMatchMaker();
            }

            networkManager.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
            status.text = "Loading ...";
        }

        public void OnMatchList(bool _success, string _extendedInfo, List<MatchInfoSnapshot> _matches)
        {
            // https://docs.unity3d.com/ScriptReference/Networking.Match.NetworkMatch.html
            status.text = "";
            if (_success )
            {
                if (_matches.Count != 0)
                {
                    Debug.Log("A list of matches was returned. The number of matches =  " + _matches.Count);
                    ClearRoomList();

                    foreach (var match in _matches)
                    {
                        GameObject _roomListItemGO = (GameObject) Instantiate(listItemPrefab);
                        _roomListItemGO.transform.SetParent(roomListParent);
                        // have a component sit on the game object that will take care of setting up name, user count
                        // as well as setting up a call back function that will join the game.

                        RoomListItem roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
                        if (roomListItem != null)
                        {
                           roomListItem.Setup(match, JoinRoom);
                        }
                        else
                        {
                           Debug.LogError("The prefab for the roomListItem is missing a component: RoomListItem.cs");  
                        }


                        roomList.Add((_roomListItemGO));
                        currentMatchData.Add((match));
                    }

                }
                else
                {
                    status.text = "There are no active rooms on the Unity MatchMaking Server!";
                    Debug.Log("No matches were returned ");
                }
            }
            else
            {
                status.text = "Unable to connect with the Unity Match Maker Service!";
                Debug.LogError("Could not connect to the Unity Match Maker");
                return;
            }
        }

        public void ClearRoomList()
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                Destroy(roomList[i]);
            }

            roomList.Clear();
            currentMatchData.Clear();
        }

        public void JoinRoom(MatchInfoSnapshot _match)
        {

            // https://docs.unity3d.com/Manual/UpgradeGuide54Networking.html
            networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
            StartCoroutine(WaitForJoin());
        }

        private IEnumerator WaitForJoin()
        {
            ClearRoomList();


            int countDown = 5;
            while (countDown > 0)
            {

                status.text = "Joining...( " + countDown + " )";

                yield return new WaitForSeconds(1f);
                countDown--;
            }

            // Failed to connect so notify the player and refresh the room list

            status.text = "We have failed to connect to the server";
            yield return new WaitForSeconds(2f);
            status.text = "";

            // We need to be sure the server has actually fully stopped this room
            MatchInfo matchInfo = networkManager.matchInfo;

            if (matchInfo != null)
            {    
                networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
                networkManager.StopHost();
            }



            RefreshRoomList();
        }
    }
}
