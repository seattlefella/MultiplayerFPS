using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HostGame : MonoBehaviour
    {
        [SerializeField]
        private uint roomSize = 6;

        [SerializeField]
        private string roomName;

        [SerializeField]
        private InputField roomNameInputText;

        public string RoomName
        {
            get{ return roomName; }
            set{ roomName = value; }
        }


        public void SetRoomName(string _name)
        {
            Debug.Log("The room name as passed into the funciton: " + _name);
            roomName = roomNameInputText.text;
        }
        public uint RoomSize
        {
            get { return roomSize; }
            set { roomSize = value; }
        }

        private NetworkManager networkManager;

        public void Start()
        {
            // Get and cache a reference to the network manager
            networkManager = NetworkManager.singleton;

            // Ensure that the matchmaker has started.
            if (networkManager.matchMaker == null)
            {
                networkManager.StartMatchMaker();
            }

        }





        public void CreateRoom( )
        {
            if (!string.IsNullOrEmpty(roomName))
            {
                 Debug.Log("We are creating room: " + roomName + " that can hold a max. of " + roomSize + "Players.");

                // Create the room
                networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);

            }
            else
            {
                Debug.LogError("We were asked to create a room with no name assigned.");
            }
        }
    }
}
