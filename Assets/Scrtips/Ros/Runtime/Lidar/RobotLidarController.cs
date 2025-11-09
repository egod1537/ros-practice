using UnityEngine;
using TriInspector;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Main
{
    public class RobotLidarController : MonoBehaviour
    {
        #region ========== Constant ==========
        [SerializeField]
        private LidarConfig config;
        #endregion ===========================

        [Required, SerializeField]
        private Transform trTip;

        private RosLidarPublisher publisher;

        float timer;
        float currentAngle;

        float scanTimer;
        private List<LidarData> scanDataBuffer = new List<LidarData>();

        private void Start()
        {
            publisher = new();
        }

        private void Update()
        {
            if (!trTip || config == null) return;

            float spinSpeedDegPerSec = 360f / config.updateIntervalSecond;
            float raysPerSecond = config.raysPerSecond;

            transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
            currentAngle = (currentAngle + spinSpeedDegPerSec * Time.deltaTime) % 360f;

            timer += Time.deltaTime;
            float interval = 1f / raysPerSecond;
            while (timer >= interval)
            {
                timer -= interval;
                Vector3 direction = trTip.forward;
                UpdateRadar(direction, currentAngle);
            }

            scanTimer += Time.deltaTime;
            if (scanTimer >= config.updateIntervalSecond)
            {
                if (scanDataBuffer.Count > 0)
                {
                    publisher.PublishTopic(new List<LidarData>(scanDataBuffer), Time.time, config.updateIntervalSecond);
                    scanDataBuffer.Clear();
                }
                scanTimer -= config.updateIntervalSecond;
            }
        }

        private void UpdateRadar(Vector3 direction, float angleDeg)
        {
            var origin = trTip.position;
            float distance;

            if (Physics.Raycast(origin, direction, out var hit, LidarConfig.range, config.mask))
            {
                Debug.DrawLine(origin, hit.point, Color.red, 0.1f);
                distance = hit.distance;
            }
            else
            {
                Debug.DrawRay(origin, direction * LidarConfig.range, Color.green, 0.1f);
                distance = float.PositiveInfinity;
            }

            scanDataBuffer.Add(new LidarData { angleDeg = angleDeg, distance = distance });
        }
    }
}