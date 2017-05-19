using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerNamePlate : MonoBehaviour
    {

        [SerializeField]
        private Text userNameText;

        [SerializeField]
        private RectTransform healthBarFill;

        [SerializeField]
        private Player player;

        [SerializeField]
        private Camera m_Camera;

        void Update()
        {
            m_Camera = Camera.main;

            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);

            userNameText.text = player.name;

            healthBarFill.localScale = new Vector3(player.GetHealthPct(), 1,1);
        }
    }
}

