using System.Collections.Generic;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

namespace Main
{
    public class OdometryPublisher
    {
        private const string TOPIC_NAME = "/odom";
        private const string FRAME_ID = "odom";
        private const string CHILD_FRAME_ID = "base_link";

        private ROSConnection ros;

        private static readonly double[] k_PerfectPoseCovariance = new double[36] {
            1e-6, 0, 0, 0, 0, 0,
            0, 1e-6, 0, 0, 0, 0,
            0, 0, 1e-6, 0, 0, 0,
            0, 0, 0, 1e-6, 0, 0,
            0, 0, 0, 0, 1e-6, 0,
            0, 0, 0, 0, 0, 1e-6
        };

        private static readonly double[] k_PerfectTwistCovariance = new double[36] {
            1e-6, 0, 0, 0, 0, 0,
            0, 1e-6, 0, 0, 0, 0,
            0, 0, 1e-6, 0, 0, 0,
            0, 0, 0, 1e-6, 0, 0,
            0, 0, 0, 0, 1e-6, 0,
            0, 0, 0, 0, 0, 1e-6
        };

        public OdometryPublisher()
        {
            ros = ROSConnection.GetOrCreateInstance();
            ros.RegisterPublisher<OdometryMsg>(TOPIC_NAME);
        }

        public void PublishTopic(List<OdometryData> datas, float timeStamp)
        {
            if (datas == null || datas.Count == 0)
                return;

            int timeStampSec = (int)timeStamp;
            uint timeStampNSec = (uint)((timeStamp - timeStampSec) * 1e9);
            TimeMsg timeMsg = new TimeMsg(timeStampSec, timeStampNSec);

            foreach (var data in datas)
            {
                Vector3 rosPosition = data.position.ToRosVector3();
                Quaternion rosOrientation = data.orientation.ToRosQuaternion();
                Vector3 rosLinear = data.linearVelocity.ToRosVector3();
                Vector3 rosAngular = data.angularVelocity.ToRosVector3();

                OdometryMsg msg = new OdometryMsg
                {
                    header = new HeaderMsg
                    {
                        stamp = timeMsg,
                        frame_id = FRAME_ID
                    },
                    child_frame_id = CHILD_FRAME_ID,
                    pose = new PoseWithCovarianceMsg
                    {
                        pose = new PoseMsg
                        {
                            position = rosPosition.ToRosPointMsg(),
                            orientation = rosOrientation.ToRosQuaternionMsg(),
                        },
                        covariance = k_PerfectPoseCovariance,
                    },
                    twist = new TwistWithCovarianceMsg
                    {
                        twist = new TwistMsg
                        {
                            linear = rosLinear.ToRosVector3Msg(),
                            angular = rosAngular.ToRosVector3Msg(),
                        },
                        covariance = k_PerfectTwistCovariance
                    }
                };

                ros.Publish(TOPIC_NAME, msg);
            }
        }
    }
}