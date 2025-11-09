using TriInspector;
using UnityEngine;

namespace Main
{
    public class RobotController : MonoBehaviour
    {
        [Required, SerializeField]

        private RobotRidarController ridarController;

        [Required, SerializeField]

        private RobotIMUController imuController;

        private float currentLinearVelocity;
        private float currentAngularVelocity;

        private void Update()
        {
            if (Mathf.Abs(currentAngularVelocity) > 0f)
            {
                float rotationDegrees = currentAngularVelocity * Mathf.Rad2Deg * Time.deltaTime;
                transform.Rotate(Vector3.up, rotationDegrees, Space.World);
            }

            if (Mathf.Abs(currentLinearVelocity) > 0f)
            {
                Vector3 delta = transform.forward * currentLinearVelocity * Time.deltaTime;
                transform.position += delta;
            }
        }

        public void SubscribeCommand(VelocityCommand command)
        {
            currentLinearVelocity = command.linearX;

            currentAngularVelocity = command.angularZ;

            if (imuController)
            {
                imuController.SetAngularVelocity(currentAngularVelocity);
            }
        }
    }
}