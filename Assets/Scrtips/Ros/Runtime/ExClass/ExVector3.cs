using RosMessageTypes.Geometry;
using UnityEngine;

namespace Main
{
    public static class ExVector3
    {
        public static PointMsg ToRosPointMsg(this Vector3 vec)
        {
            return new PointMsg(vec.x, vec.y, vec.z);
        }

        public static Vector3Msg ToRosVector3Msg(this Vector3 vec)
        {
            return new Vector3Msg(vec.x, vec.y, vec.z);
        }

        public static Vector3 ToRosVector3(this Vector3 v)
        {
            return new Vector3(v.z, -v.x, v.y);
        }
    }
}