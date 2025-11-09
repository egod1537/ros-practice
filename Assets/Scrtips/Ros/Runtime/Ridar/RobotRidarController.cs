using UnityEngine;
using TriInspector;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Main
{
    public class RobotRidarController : MonoBehaviour
    {
        #region ========== Event ==========
        public UnityEvent<List<RidarData>> onPublishData = new();
        #endregion ========================

        #region ========== Constant ==========
        [SerializeField]
        private RidarConfig config;
        #endregion ===========================

        [Required, SerializeField] 
        private Transform trTip;

        float timer;
        float currentAngle;
        
        float scanTimer;
        private List<RidarData> scanDataBuffer = new List<RidarData>();

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
                    onPublishData.Invoke(new List<RidarData>(scanDataBuffer));
                    scanDataBuffer.Clear();
                }
                scanTimer -= config.updateIntervalSecond;
            }
        }

        private void UpdateRadar(Vector3 direction, float angleDeg)
        {
            var origin = trTip.position;
            float distance;

            if (Physics.Raycast(origin, direction, out var hit, RidarConfig.range, config.mask))
            {
                Debug.DrawLine(origin, hit.point, Color.red, 0.1f);
                distance = hit.distance;
            }
            else
            {
                Debug.DrawRay(origin, direction * RidarConfig.range, Color.green, 0.1f);
                distance = float.PositiveInfinity;
            }
            
            scanDataBuffer.Add(new RidarData { angleDeg = angleDeg, distance = distance });
        }
    }
}