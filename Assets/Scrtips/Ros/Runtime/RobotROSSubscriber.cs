using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

namespace Main
{
    public class RobotROSSubscriber : MonoBehaviour
    {
        [SerializeField]
        private RobotController robotController;

        private void Start()
        {
            ROSConnection.GetOrCreateInstance().Subscribe<TwistMsg>(
                "cmd_vel",
                Callback
            );
        }

        private void Callback(TwistMsg msg)
        {
            Debug.Log($"Received from ROS: {msg.linear} {msg.angular}");
            robotController.SubscribeCommand(new VelocityCommand(msg));
        }
    }
}