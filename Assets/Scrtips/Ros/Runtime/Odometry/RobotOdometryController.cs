using System.Collections.Generic;
using Codice.CM.Common.Replication;
using PlasticGui.Configuration;
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

namespace Main
{
    public class RobotOdometryController : MonoBehaviour
    {
        private const string ODOMETRY_TOPIC_NAME = "/cmd_vel";

        private float currentLinearX = 0f;
        private float currentAngularZ = 0f;

        private float currentX = 0f;
        private float currentY = 0f;
        private float currentTheta = 0f;

        [SerializeField]
        private OdometryConfig config;

        private ROSConnection ros;

        private OdometryPublisher publisher;
        private float publishTimer;

        private readonly List<OdometryData> odometryDataBuffer = new(1);

        private void Start()
        {
            ros = ROSConnection.GetOrCreateInstance();
            ros.Subscribe<TwistMsg>(ODOMETRY_TOPIC_NAME, OnReceiveTwist);
            publisher = new();
        }

        private void Update()
        {
            if (publisher == null || config == null)
                return;

            publishTimer += Time.deltaTime;

            if (publishTimer >= config.updateIntervalSecond)
            {
                float timeStamp = Time.time;

                odometryDataBuffer.Clear();
                odometryDataBuffer.Add(getCurrentOdometryData(timeStamp));

                publisher.PublishTopic(odometryDataBuffer, timeStamp);

                publishTimer -= config.updateIntervalSecond;
            }
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            float deltaX = currentLinearX * Mathf.Cos(currentTheta) * dt;
            float deltaY = currentLinearX * Mathf.Sin(currentTheta) * dt;
            float deltaTheta = currentAngularZ * dt;

            currentX += deltaX;
            currentY += deltaY;
            currentTheta = (currentTheta + deltaTheta) % (2 * Mathf.PI);
        }

        private void OnReceiveTwist(TwistMsg msg)
        {
            float unityAngularZ = -(float)msg.angular.z;

            currentLinearX = (float)msg.linear.x;
            currentAngularZ = unityAngularZ;
        }

        private OdometryData getCurrentOdometryData(float timeStamp)
        {
            return new OdometryData
            {
                position = new Vector3(currentX, currentY, 0),
                orientation = Quaternion.Euler(0, 0, currentTheta * Mathf.Rad2Deg),
                linearVelocity = new Vector3(currentLinearX, 0, 0),
                angularVelocity = new Vector3(0, 0, currentAngularZ),
                timeStamp = timeStamp
            };
        }
    }
}