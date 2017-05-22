using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent((typeof(ConfigurableJoint)))]
    [RequireComponent(typeof(PlayerMotor))]
    [RequireComponent((typeof(Animator)))]
    public class PlayerController : MonoBehaviour
    {
        // Player movement settings
        [Header("Player Movement Options:")]
        [SerializeField]
        private float speed = 5f;
        // How fast we can look left/right/up/down
        [SerializeField]
        private float lookSensitivity = 3f;

        [Header("Thruster Options:")]
        [SerializeField]
        private float thrusterForce = 1000f;

        [SerializeField]
        private float thrusterFuelBurnSpeed = 0.8f;

        [SerializeField]
        private float thrusterFuelRegenSpeed = 0.3f;

        private float thrusterFuelAmount = 1f;


        [SerializeField]
        private LayerMask environmentLayerMask;



        // We will use a "joint" component to allow our player to drift back down to a fixed height after using his thrusters
        [Header("Joint Options:")]
        [SerializeField]
        private JointProjectionMode jointProjectionMode = JointProjectionMode.PositionAndRotation;

        [SerializeField]
        private float jointSpring = 20f;

        [SerializeField]
        private float jointMaxForce = 40f;



        // Internal references
        private PlayerMotor motor;
        private ConfigurableJoint joint;
        private Animator animator;

        // -------------------------Private variables needed by the methods-------------------------------------

        // Player raw device input
        private float xMov = 0f;
        private float zMov = 0f;
        private float yRot = 0f;
        private float xRot = 0f;

        // basic movement of our player
        private Vector3 movHorzontal = Vector3.zero;
        private Vector3 movVertical = Vector3.zero;
        private Vector3 velocity = Vector3.zero;

        // We must check how high our player is above the ground using a RayCast
        private RaycastHit hit;


        // Variables needed for player and camera rotation
        private Vector3 rotation = Vector3.zero;
        private float cameraRotationX;




        void Start()
        {
            motor = GetComponent<PlayerMotor>();
            joint = GetComponent<ConfigurableJoint>();
            animator = GetComponent<Animator>();

            SetJointSettings(jointSpring);

        }

        void Update()
        {
            // If the player has paused the game do not do anything !
            if (PauseMenu.IsOn)
            {
                if (Cursor.lockState != CursorLockMode.None)
                {
                    Cursor.lockState = CursorLockMode.None;
                }

                // We need to lock down the player in case he was holding a WASD key down when he entered the Pause mode
                motor.Move(Vector3.zero);
                motor.Rotate(Vector3.zero);
                motor.RotateCamera(0f);

                return;
            }

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // this sets the target position of the spring-Joint when we are flying over objects
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, environmentLayerMask))
            {
                joint.targetPosition = new Vector3(0f, -hit.point.y, 0f);
            }
            else
            {
                joint.targetPosition = new Vector3(0f, 0, 0f);
            }

            //Calculate movement  as a  3D velocity vector
            xMov = Input.GetAxis("Horizontal");
            zMov = Input.GetAxis("Vertical");

            movHorzontal = transform.right * xMov;
            movVertical = transform.forward * zMov;

            // Final movement Vector
            velocity = (movVertical +movHorzontal) * speed;

            // Animate thruster movement
            animator.SetFloat("ForwardVelocity",zMov);

            //Apply movement
            motor.Move(velocity);

            // Calculate rotations as a 3d Vector (turning the player around)
            yRot = Input.GetAxisRaw("Mouse X");
            rotation = new Vector3(0, yRot, 0f)* lookSensitivity;

            // Apply rotation
            motor.Rotate(rotation);

            // Calculate rotations as a 3d Vector (turning the camera around)
            xRot = Input.GetAxisRaw("Mouse Y");
            cameraRotationX = xRot * lookSensitivity;

            // Apply Camera rotation
            motor.RotateCamera(cameraRotationX);

            // Calculate the thruster force
            Vector3 calcThrusterForce = Vector3.zero;
            if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
            {
                // We must consume fuel to jump
                thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

                // Only apply force if there is greater than zero fuel
                if (thrusterFuelAmount >= 0.1f)
                {
                    calcThrusterForce = Vector3.up * thrusterForce;
                    SetJointSettings(0f);  
                }


            }
            else
            {
                // We regenerate fuel when we are not jumping
                thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

                // if we are not using the thrusters return the spring
                SetJointSettings(jointSpring);
            }

            // Let's keep the fuel level between zero and one
            thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

            // Apply Thrusters
            motor.ApplyThruster(calcThrusterForce);

        }

        private void  SetJointSettings(float _jointSpring)
        {
            joint.yDrive = new JointDrive
            {
                maximumForce = jointMaxForce,
                positionSpring = _jointSpring
            };

            // As the mode never changes this is not really needed
            joint.projectionMode = jointProjectionMode;


        }

        public float GetThrusterFuelAmount()
        {
            return thrusterFuelAmount;
        }


    }
}

