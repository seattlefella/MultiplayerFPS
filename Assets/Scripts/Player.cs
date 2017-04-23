//-----------------------------------------------------------------------
// This is the primary script that controls all players in the network.
// Including: local and remote.
// In some ways this script is really two scripts in one.  The local Client
// and the remote client.
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerSetup))]
    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;

        [SerializeField]
        [SyncVar]
        private int currentHealth = 100;

        [SyncVar]
        public string UserName = "Loading...";

        // This flag shows if the player has health greater than zero
        [SerializeField]
        [SyncVar]
        private bool isDead = false;

        public int Kills;
        public int Deaths;


        public bool IsDead
        {
            get { return isDead; }
            protected set { isDead = value; }
        }

        [SerializeField]
        private GameObject[] disableGameObjectsOnDeath;

        [SerializeField]
        private Behaviour[] disableOnDeath;

        [SerializeField]
        private GameObject deathEffect;

        [SerializeField]
        private GameObject spawnPlayerEffect;

        [SerializeField]
        private bool[] wasEnabled;

        private Collider playerCollider;
        private Transform spawnPoint;

        private bool isFirstSetup = true;

        // Scrap and loop counting variables
        private int i, j, k;
        //----------------------For test only--------------------

        public bool IsServer = false;
        public bool IsClient = false;
        public bool IsLocalPlayer = false;
        public bool HasAuthority = false;

        void Update()
        {
            IsServer = this.isServer;
            IsClient = this.isClient;
            IsLocalPlayer = this.isLocalPlayer;
            HasAuthority = this.hasAuthority;


            if (!isLocalPlayer)
            {
                return;
            }

            //if(Input.GetKeyDown(KeyCode.K))
            //{
            //    RpcTakeDamage(2000);
            //}
        }

        public void SetupPlayer()
        {
            if (isLocalPlayer)
            {
                // Upon player creation switch from the scene camera to the player camera.
                // And, turn the playerUI back on
                // Note: this is for the local player only. 
                GameManager.Instance.SetSceneCameraActive(false);
                GetComponent<PlayerSetup>().PlayerUiInstance.SetActive(true);          
            }

            CmdBroadCastNewPlayerSetup();
        }

        [Command]
        private void CmdBroadCastNewPlayerSetup()
        {
            RpcSetupPlayerOnAllClients();
        }

        [ClientRpc]
        private void RpcSetupPlayerOnAllClients()
        {
            if (isFirstSetup)
            {
                 wasEnabled = new bool[disableOnDeath.Length];
                for (i = 0; i < wasEnabled.Length; i++)
                {
                    wasEnabled[i] = disableOnDeath[i].enabled;
                }

                isFirstSetup = false;
            }

            SetDefaults();
        }

        [ClientRpc]
        public void RpcTakeDamage(int _amount, string _sourceID)
        {
            //If the player is dead there is no more damage to be given
            if (isDead)
            {
                return;
            }

            currentHealth -= _amount;
            Debug.Log(transform.name + " now has health of: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die(_sourceID);
            }
        }

        private void Die(string _sourceID)
        {
            isDead = true;
            Player sourcePlayer = GameManager.GetPlayer(_sourceID);
            if (sourcePlayer != null)
            {
                sourcePlayer.Kills++;
            }
            Deaths++;


            // We must disable some components on the player object. IE. he should not be able to move; shoot; or been seen for that matter;

            for ( i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = false;
            }

            for (i = 0; i < disableGameObjectsOnDeath.Length; i++)
            {
                disableGameObjectsOnDeath[i].SetActive(false);
            }

            // disable the collider so nothing that is still active in the scene can interact with the dead player
            playerCollider = GetComponent<Collider>();
            if (playerCollider != null)
            {
                playerCollider.enabled = true;
            }

            // Spawn a death effect - explosion, smoke,...
            GameObject _deathEffectGfx = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(_deathEffectGfx,3f);

            // Upon death switch back to the scene camera and turn off the player UI
            if (isLocalPlayer)
            {
                GameManager.Instance.SetSceneCameraActive(true);
                GetComponent<PlayerSetup>().PlayerUiInstance.SetActive(false);
            }

            Debug.Log(transform.name + " Has just died!! God rest his or her soul!!!" );

            // Bring the player back from the dead.
            StartCoroutine(respawn());
        }

        private IEnumerator respawn()
        {
            yield return new WaitForSeconds(GameManager.Instance.MatchSettings.ReSpawnTime);

            spawnPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            // we want the clients to stabilize prior to sending updates to all clients.
            yield return new WaitForSeconds(0.1f);

            SetupPlayer();

            Debug.Log(transform.name + " has been ReSpawned!");

        }

        public void SetDefaults()
        {
            // Upon waking up and establish a fully healthy player
            isDead = false;
            currentHealth = maxHealth;


            // As we are bringing the player back to life we must re-enable those components that we turned off on death.
            for ( i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = wasEnabled[i];
            }

            // As we are bringing the player back to life we must re-enable those GameObjects that we turned off on death.
            for (i = 0; i < disableGameObjectsOnDeath.Length; i++)
            {
                disableGameObjectsOnDeath[i].SetActive(true);
            }

            // collider's are not derived from behaviors so we must do them separately
            // note: This code will  work if there is only one collider.  It could happen that the player has several collider(s) ie. different targets on his body.
            playerCollider = GetComponent<Collider>();
            if (playerCollider != null)
            {
                playerCollider.enabled = true;
            }

            // As our player comes to life we will use a visual effect to highlight it.
            // Spawn a death effect - explosion, smoke,...
            GameObject _spawnPlayerEffect = (GameObject)Instantiate(spawnPlayerEffect, transform.position, Quaternion.identity);
            Destroy(_spawnPlayerEffect, 3f);

        }

    }
}
