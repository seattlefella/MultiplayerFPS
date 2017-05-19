using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerUI : MonoBehaviour
    {

        [SerializeField]
        private RectTransform thrusterFuelFill;

        [SerializeField]
        private RectTransform healthBarFill;

        [SerializeField] private Text ammoText;

        private Vector3 fuelLevel = new Vector3(1f,1f,1f);
        private Vector3 healthLevel = new Vector3(1f, 1f, 1f);
        private PlayerController controller;
        private WeaponManager weaponManager;

        private Player player;

        [SerializeField]
        private GameObject pauseMenu;

        [SerializeField]
        private GameObject scoreBoard;

        void SetFuelAmount(float _amount)
        {
            fuelLevel.y = _amount;
            thrusterFuelFill.localScale = fuelLevel;
        }


        private void SetHealthAmount(float _amount)
        {
            healthLevel.y = _amount;
            healthBarFill.localScale = healthLevel;
        }

        public void SetPlayer(Player _player)
        {
            if (_player == null)
            {
                Debug.LogError("There was no player sent as an arg in SetPlayer in the PlayerUI.cs");
            }
            player = _player;
            controller = player.GetComponent<PlayerController>();
            weaponManager = player.GetComponent<WeaponManager>();
        }

        void Start()
        {
            PauseMenu.IsOn = false;
            pauseMenu.SetActive(false);
        }

        void Update()
        {
            if (controller != null)
            {
                 SetFuelAmount(controller.GetThrusterFuelAmount());
            }

            if (player != null)
            {
             SetHealthAmount(player.GetHealthPct());               
            }

            if (weaponManager != null)
            {
             SetAmmoAmount(weaponManager.GetCurrentWeapon().Bullets);               
            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleScoreMenu();
            }
        }

        private void SetAmmoAmount(int _amount)
        {
            if (ammoText != null)
            {
              ammoText.text = _amount.ToString();              
            }

        }


        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseMenu.IsOn = pauseMenu.activeSelf;
        }

        public void ToggleScoreMenu()
        {
            scoreBoard.SetActive(!scoreBoard.activeSelf);
            ScoreBoard.IsOn = scoreBoard.activeSelf;
        }
    }
}
