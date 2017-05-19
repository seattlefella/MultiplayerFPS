using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerNamePlate : MonoBehaviour
    {

        [SerializeField]
        private Text userNameText;

        [SerializeField]
        private Player player;

        void Update()
        {
            userNameText.text = player.name;
        }
    }
}
