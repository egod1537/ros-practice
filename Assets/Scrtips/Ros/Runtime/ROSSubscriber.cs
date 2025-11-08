using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using UnityEngine;
using RosMessageTypes.Std;

public class ROSSubscriber : MonoBehaviour
{
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<StringMsg>(
            "ros_test",
            Callback
        );
    }

    void Callback(StringMsg msg)
    {
        Debug.Log($"Received from ROS: {msg.data}");
    }
}
