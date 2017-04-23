using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerUI : MonoBehaviour
    {

        [SerializeField]
        private RectTransform thrusterFuelFill;

        private Vector3 fuelLevel = new Vector3(1f,1f,1f);
        private PlayerController controller;

        [SerializeField]
        private GameObject pauseMenu;

        [SerializeField]
        private GameObject scoreBoard;

        void SetFuelAmount(float _amount)
        {
            fuelLevel.y = _amount;
            thrusterFuelFill.localScale = fuelLevel;
        }

        public void SetController(PlayerController _controller)
        {
            controller = _controller;
        }

        void Start()
        {
            PauseMenu.IsOn = false;
            pauseMenu.SetActive(false);
        }

        void Update()
        {
            SetFuelAmount(controller.GetThrusterFuelAmount());

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleScoreMenu();
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
