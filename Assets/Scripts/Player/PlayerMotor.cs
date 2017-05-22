using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

        private Vector3 velocity = Vector3.zero;
        [SerializeField]
        private Vector3 rotation = Vector3.zero;
        private float cameraRotationX = 0f;
        private float currentCameraRotationX = 0f;

        [SerializeField] private float cameraRotationLimit = 85f;

        private Vector3 thrusterForce = Vector3.zero;

        private Rigidbody rb;


        void Start()
        {
            rb = GetComponent<Rigidbody>();

        }
        public void Move(Vector3 _velocity)
        {
            velocity = _velocity;

        }

        // Run every Physics iterations
        void FixedUpdate()
        {
            PerformMovement();
            PerformRotation();
            PerformCameraRotation();
        }

        void PerformMovement()
        {
            if (velocity != Vector3.zero)
            {
                rb.MovePosition(rb.position+velocity*Time.fixedDeltaTime);
            }

            if (thrusterForce != Vector3.zero)
            {
                rb.AddForce(thrusterForce*Time.fixedDeltaTime, ForceMode.Acceleration);
            }

        }

        void PerformRotation()
        {
            if (rotation != Vector3.zero)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
            }

        }

        void PerformCameraRotation()
        {
            if (cam != null)
            {
                // Set the camera rotation and clamp it to a designer selected range
                currentCameraRotationX -= cameraRotationX;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX,-cameraRotationLimit, cameraRotationLimit);
                // Apply the calculated rotation to the transform of the camera
                cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
            }

        }

        public void Rotate(Vector3 _rotation)
        {
            rotation = _rotation;
        }

        public void RotateCamera(float _cameraRotationX)
        {
            cameraRotationX = _cameraRotationX;
        }

        // Get a force vector for our thruster
        public void ApplyThruster(Vector3 _thrusterForce)
        {
            thrusterForce = _thrusterForce;
        }
    }
}


