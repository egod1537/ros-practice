using UnityEngine;

namespace Main
{
    public struct ImuData
    {
        public Quaternion orientation;
        public Vector3 angularVelocity;
        public Vector3 linearAcceleration;
    }
}