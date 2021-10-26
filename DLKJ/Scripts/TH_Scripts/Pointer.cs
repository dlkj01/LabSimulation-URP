using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DLKJ.InstrumentAction;
namespace DLKJ
{
    public class Pointer : MonoBehaviour
    {
        public float currentAngle;
        public string deviceName;
        PressButtonGroup group;
        private void Start()
        {
            Item item = SceneManager.GetInstance().GetItemByName(deviceName);
            if (item != null)
            {
                group = item.GetComponent<InstrumentAction>().group;
            }
        }

        public void SetAngle(float value)
        {
            if (float.IsNaN(value)) return;
            float range = maxValue - minValue;
            float everyAngle = (maxAngle - minAngle) / range;
            currentAngle = everyAngle * (value) + minAngle;
            if (group != null)
            {
                currentAngle *= group.GetValue();
            }

        }
        public float rotateSpeed;
        public float minValue;
        public float maxValue;
        public float minAngle;
        public float maxAngle;
        public RotationType rotationType = RotationType.X_AxisRotation;
        public void PointerRotate()
        {
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            switch (rotationType)
            {
                case RotationType.Y_AxisRotation:
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Vector3.up * currentAngle), Time.deltaTime * rotateSpeed);
                    break;
                case RotationType.X_AxisRotation:
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Vector3.right * currentAngle), Time.deltaTime * rotateSpeed);
                    break;
                case RotationType.Z_AxisRotation:
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Vector3.forward * currentAngle), Time.deltaTime * rotateSpeed);
                    break;
                default:
                    break;
            }
        }
    }
}
