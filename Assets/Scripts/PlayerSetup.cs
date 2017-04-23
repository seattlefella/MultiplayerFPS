//-----------------------------------------------------------------------
// Responsible for setting up the player.
// Including: Adding/removing him from the network; setting up the UI; setting personal
// variables such as name, camera, ID
//-----------------------------------------------------------------------
//24:30 lesson: 28

using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField]
        private Behaviour[] ComponentsToDisable;

        [SerializeField]
        private string remoteLayerName = "RemotePlayer";

        [SerializeField]
        private string doNotDrawLayerName = "DoNotDraw";

        //[SerializeField]
        //private string id = "TBD";


        [SerializeField]
        private GameObject playerGraphics;

        [SerializeField]
        private GameObject playerUIPrefab;
        [HideInInspector]
        public GameObject PlayerUiInstance;

        private Player player;
        private string netID;

        void Start()
        {
            if (!isLocalPlayer)
            {
                DisableComponents();
                AssignRemoteLayer();
            }

            else

            {
                // Disable player Graphics for the local player
                SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(doNotDrawLayerName));

                // Create player UI
                PlayerUiInstance = Instantiate(playerUIPrefab);
                PlayerUiInstance.name = playerUIPrefab.name;

                // Configure Player UI
                PlayerUI ui = PlayerUiInstance.GetComponent<PlayerUI>();

                if (ui == null)
                {
                    Debug.LogError("There is no PlayerUI component on: " + PlayerUiInstance.name + " PlayerUI prefab.");
                }
                else
                {
                   ui.SetController(GetComponent<PlayerController>());
                }

                GetComponent<Player>().SetupPlayer();


                var _username = "Loading...";
                if (UserAccountManager.IsLoggedIn)
                {
                    _username = UserAccountManager.PlayerUsername;
                }

                else
                {
                    _username = transform.name;
                }

                CmdSetUserName(transform.name, _username);
            }
        }


        [Command]
        void CmdSetUserName(string _playerID, string _userName)
        {
            Player playerTmp = GameManager.GetPlayer(_playerID);
            if (playerTmp != null)
            {
              Debug.Log("Player: " + _userName + " has joined the room");    
              playerTmp.UserName = _userName;
            }
        }


        private void SetLayerRecursively(GameObject _playerGraphics, int _layer)
        {
            _playerGraphics.layer = _layer;

            foreach (Transform child in _playerGraphics.transform)
            {
                SetLayerRecursively(child.gameObject, _layer);
            }

        }


        public override void OnStartClient()
        {
            base.OnStartClient();
            netID = GetComponent<NetworkIdentity>().netId.ToString();
            player = GetComponent<Player>();

            GameManager.RegisterPlayer(netID, player);
        }

        void DisableComponents()

        {
            for (var i = 0; i < ComponentsToDisable.Length; i++)
            {
                ComponentsToDisable[i].enabled = false;
            }
        }

        void AssignRemoteLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        }

        void OnDisable()
        {

            // Let's get rid of the playerUIInsatance
            Destroy(PlayerUiInstance);


            if (isLocalPlayer)
            {
             GameManager.Instance.SetSceneCameraActive(true);              
            }
 

            GameManager.UnRegisterPlayer(transform.name);
        }


    }
}

