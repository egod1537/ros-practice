
using UnityEngine;

namespace Main
{
    public struct OdometryData
    {
        public Vector3 position;
        public Quaternion orientation;
        public Vector3 linearVelocity;
        public Vector3 angularVelocity;
        public float timeStamp;
    }
}