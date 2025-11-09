using RosMessageTypes.Geometry;
using UnityEngine;

namespace Main
{
    public static class ExQuaternion
    {
        public static QuaternionMsg ToRosQuaternionMsg(this Quaternion q)
        {
            return new QuaternionMsg(q.x, q.y, q.z, q.w);
        }

        public static Quaternion ToRosQuaternion(this Quaternion q)
        {
            return new Quaternion(q.z, -q.x, q.y, -q.w);
        }
    }
}