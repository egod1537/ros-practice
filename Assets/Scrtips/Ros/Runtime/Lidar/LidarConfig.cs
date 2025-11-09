using System;
using UnityEngine;

namespace Main
{
    [Serializable]
    public class LidarConfig
    {
        public float updateIntervalSecond = 1f;
        public int raysPerSecond = 360;
        public const float range = 1000f;
        public LayerMask mask = ~0;
    }
}