using System.Collections.Generic;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

namespace Main
{
    public class RosIMUPublisher
    {
        private const string TOPIC_NAME = "/imu/data";
        private const string FROM_ID = "imu_link";

        private static readonly double[] k_PerfectCovariance = new double[9] { 1e-6, 0, 0, 0, 1e-6, 0, 0, 0, 1e-6 };

        private uint seq = 0;

        private ROSConnection ros;

        public RosIMUPublisher()
        {
            ros = ROSConnection.GetOrCreateInstance();
            ros.RegisterPublisher<ImuMsg>(TOPIC_NAME);
        }

        public void PublishTopic(List<ImuData> imuDatas, float timeStamp)
        {
            if (imuDatas == null || imuDatas.Count == 0) return;

            int timeStampSec = (int)timeStamp;
            uint timeStampNsec = (uint)((timeStamp - timeStampSec) * 1e9);
            TimeMsg timeMsg = new TimeMsg(timeStampSec, timeStampNsec);

            foreach (var data in imuDatas)
            {
                ImuMsg msg = new ImuMsg
                {
                    header = new HeaderMsg
                    {
                        stamp = timeMsg,
                        frame_id = FROM_ID
                    },
                    orientation = new QuaternionMsg
                    {
                        x = data.orientation.x,
                        y = data.orientation.y,
                        z = data.orientation.z,
                        w = data.orientation.w,
                    },
                    angular_velocity = new Vector3Msg
                    {
                        x = data.angularVelocity.x,
                        y = data.angularVelocity.y,
                        z = data.angularVelocity.z,
                    },
                    linear_acceleration = new Vector3Msg
                    {
                        x = data.linearAcceleration.x,
                        y = data.linearAcceleration.y,
                        z = data.linearAcceleration.z,
                    },

                    orientation_covariance = k_PerfectCovariance,
                    angular_velocity_covariance = k_PerfectCovariance,
                    linear_acceleration_covariance = k_PerfectCovariance,
                };

                ros.Publish(TOPIC_NAME, msg);
            }
        }
    }
}