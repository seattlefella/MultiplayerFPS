using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Player))]
    public class PlayerScore : MonoBehaviour
    {

        [SerializeField] private Player player;

        private int lastKills = 0;
        private int lastDeaths = 0;
        public void Start()
        {
            player = GetComponent<Player>();
            StartCoroutine(SyncScoreLoop());
        }

        IEnumerator SyncScoreLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(5.0f);
                SyncNow();
            }

        }

        void OnDestroy()
        {
            if (player != null)
            {
                SyncNow();
            }
        }

        public void SyncNow()
        {
            if (UserAccountManager.IsLoggedIn)
            {
                UserAccountManager.instance.GetUserData(OnDataRecieved);
            }
        }

        public void OnDataRecieved(string _data)
        {

            if (player.Kills <= lastKills && player.Deaths <= lastDeaths)
            {
                return;
            }

            int killsSinceLast = player.Kills - lastKills;
            int deathsSinceLast = player.Deaths - lastDeaths;


            int kills = DataTranslator.DataToKills(_data);
            int deaths = DataTranslator.DataToDeaths(_data);

            int updatedKills = killsSinceLast + kills;
            int updatedDeaths = deathsSinceLast + deaths;

            string newData = DataTranslator.ValueToData(updatedKills, updatedDeaths);

            Debug.Log("The data string we are sending to the database is: " + newData + "for player: " + player.name);

            lastDeaths = player.Deaths;
            lastKills = player.Kills;

            UserAccountManager.instance.SendData(newData);
        }

    }
}
