using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RoomListItem : MonoBehaviour
    {

        // We must create a call back delegate to allow us to join a selected room
        public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);

        private JoinRoomDelegate joinRoomCallback;

        //   private MatchDesc match1;
        private MatchInfoSnapshot match;

        [SerializeField] private Text roomNameText;

        public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
        {
            match = _match;
            roomNameText.text = match.name + "  (" + match.currentSize + "/" + match.maxSize + ")";

            // Get the call back function from the code that calls Setup()
            joinRoomCallback = _joinRoomCallback;
        }

        public void JoinRoom()
        {
            joinRoomCallback.Invoke(match);
        }
    }

}

