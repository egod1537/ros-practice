using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Main
{
    public class RobotIMUController : MonoBehaviour
    {
        #region ========== Event ==========
        public UnityEvent<List<ImuData>> onPublishData = new();
        #endregion ========================

        [SerializeField]
        private ImuConfig config;

        private RosIMUPublisher publisher;

        private float publishTimer;
        private float sampleTimer;
        private List<ImuData> imuDataBuffer = new List<ImuData>();

        private float currentAngularZ_rad_s = 0f;

        private void Start()
        {
            publisher = new();
        }

        #region ========== Life Cycle ==========
        private void Update()
        {
            if (config == null || config.updateIntervalSecond <= 0 || config.samplesPerSecond <= 0)
                return;

            float sampleInterval = 1f / config.samplesPerSecond;
            sampleTimer += Time.deltaTime;
            while (sampleTimer >= sampleInterval)
            {
                sampleTimer -= sampleInterval;
                SampleImuData();
            }

            publishTimer += Time.deltaTime;
            if (publishTimer >= config.updateIntervalSecond)
            {
                if (imuDataBuffer.Count > 0)
                {
                    publisher.PublishTopic(new List<ImuData>(imuDataBuffer), Time.time);
                    imuDataBuffer.Clear();
                }
                publishTimer -= config.updateIntervalSecond;
            }
        }

        private void SampleImuData()
        {
            Quaternion unityOrientation = transform.rotation;
            Vector3 unityAngularVelocity = new Vector3(0, currentAngularZ_rad_s, 0);
            Vector3 unityLinearAcceleration = Vector3.up * config.gravity;

            ImuData data = new ImuData
            {
                orientation = ToRosQuaternion(unityOrientation),
                angularVelocity = ToRosVector3(unityAngularVelocity),
                linearAcceleration = ToRosVector3(unityLinearAcceleration)
            };

            imuDataBuffer.Add(data);
        }
        #endregion ========================

        public void SetAngularVelocity(float angularZ_rad_s)
        {
            currentAngularZ_rad_s = angularZ_rad_s;
        }

        private Vector3 ToRosVector3(Vector3 v)
        {
            return new Vector3(v.z, -v.x, v.y);
        }

        private Quaternion ToRosQuaternion(Quaternion q)
        {
            return new Quaternion(q.z, -q.x, q.y, -q.w);
        }
    }
}