using System;
using System.Collections.Generic;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Main
{
    public class RosLidarPublisher
    {
        private const string TOPIC_NAME = "/scan";
        private const string FROM_ID = "lidar_link";
        private const float RANGE_MIN = 0.1f;
        private const float RANGE_MAX = float.MaxValue;

        private ROSConnection ros;
        private float[] rangesBuffer;
        private float[] intensitiesBuffer;

        public RosLidarPublisher()
        {
            ros = ROSConnection.GetOrCreateInstance();
            ros.RegisterPublisher<LaserScanMsg>(TOPIC_NAME);
        }

        public void PublishTopic(List<LidarData> lidarDatas, float timeStamp, float scanTime)
        {
            if (lidarDatas == null || lidarDatas.Count == 0)
                return;

            int numRanges = lidarDatas.Count;

            if (rangesBuffer == null || rangesBuffer.Length != numRanges)
            {
                rangesBuffer = new float[numRanges];
                intensitiesBuffer = new float[numRanges];
            }

            for (int i = 0; i < numRanges; i++)
                rangesBuffer[i] = lidarDatas[i].distance;

            int timeStampSec = (int)timeStamp;
            uint timeStampNsec = (uint)((timeStamp - timeStampSec) * 1e9);
            TimeMsg timeMsg = new TimeMsg(timeStampSec, timeStampNsec);

            float angleIncrementRad = (2 * Mathf.PI) / numRanges;

            LaserScanMsg msg = new LaserScanMsg
            {
                header = new HeaderMsg
                {
                    stamp = timeMsg,
                    frame_id = FROM_ID,
                },
                angle_min = 0.0f,
                angle_max = (2.0f * Mathf.PI) - angleIncrementRad,
                angle_increment = angleIncrementRad,
                time_increment = 0.0f,
                scan_time = scanTime,
                range_min = RANGE_MIN,
                range_max = RANGE_MAX,
                ranges = rangesBuffer,
                intensities = intensitiesBuffer,
            };

            ros.Publish(TOPIC_NAME, msg);
        }
    }
}