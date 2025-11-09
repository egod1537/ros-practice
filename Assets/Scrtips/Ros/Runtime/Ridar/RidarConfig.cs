using System;
using UnityEngine;

namespace Main
{
    [Serializable]
    public class RidarConfig
    {
        public float updateIntervalSecond = 1f;
        public int raysPerSecond = 360;
        public const float range = float.MaxValue;
        public LayerMask mask = ~0;
    }
}