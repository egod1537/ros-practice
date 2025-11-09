using System;
using UnityEngine;

namespace Main
{
    [Serializable]
    public class ImuConfig
    {
        public float updateIntervalSecond = 0.05f;

        public float samplesPerSecond = 100f;

        public float gravity = 9.81f;
    }
}