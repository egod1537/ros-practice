using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using RosMessageTypes.Std;

public class UnityToROS : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "unity_test";

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<StringMsg>(topicName);

        InvokeRepeating(nameof(PublishMsg), 1f, 1f);
    }

    void PublishMsg()
    {
        var msg = new StringMsg("Hello from Unity!");
        ros.Publish(topicName, msg);
    }

    public void Test()
    {
        Debug.Log("hihihi");
    }
}
