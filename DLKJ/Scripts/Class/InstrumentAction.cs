using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace DLKJ
{
    public partial class InstrumentAction : MonoBehaviour
    {
        [Header("Pointer Settings")]
        public Transform pointer;

        [Header("Power Material")]
        [SerializeField] Material powerMaterial;
        [SerializeField] Color onColor;
        [SerializeField] Color offColor;
        public enum RotationType { Y_AxisRotation, X_AxisRotation, Z_AxisRotation };
        public RotationType rotationType = RotationType.X_AxisRotation;
        /// <summary>
        /// 模型旋转速度
        /// </summary>
        public float RotationSpeed = 2;
        /// <summary>
        /// 当前步长
        /// </summary>
        private float _stepLength = 0;
        public float StepLength
        {
            get
            {
                return _stepLength;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > TempMaxStep)
                {
                    value = TempMaxStep;
                }
                _stepLength = value;
            }
        }
        private float TempMaxStep;

        /// <summary>
        /// 根据数值返回角度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float ReturnAngle(float value)
        {
            return -45f + value * 0.9f;
        }
        /// <summary>
        /// 设置步长
        /// </summary>
        /// <param name="value"></param>
        public void SetStepLength(float value)
        {
            StepLength = value;
        }
        public void ClearValue()
        {
            StepLength = 0;
            foreach (var item in instrumentButton)
            {
                item.rotary = 0;
            }
        }
        public float TestValue;
        public void PointerRotate()
        {
            if (pointer != null)
            {
                switch (rotationType)
                {
                    case RotationType.Y_AxisRotation:
                        pointer.localRotation = Quaternion.Lerp(pointer.localRotation, Quaternion.Euler(Vector3.up * (ReturnAngle(StepLength) % 360)), Time.deltaTime * RotationSpeed);
                        break;
                    case RotationType.X_AxisRotation:
                        pointer.localRotation = Quaternion.Lerp(pointer.localRotation, Quaternion.Euler(Vector3.right * (ReturnAngle(StepLength) % 360)), Time.deltaTime * RotationSpeed);
                        break;
                    case RotationType.Z_AxisRotation:
                        pointer.localRotation = Quaternion.Lerp(pointer.localRotation, Quaternion.Euler(Vector3.forward * (ReturnAngle(StepLength) % 360)), Time.deltaTime * RotationSpeed);
                        break;
                    default:
                        break;
                }
            }
        }

        private void Awake()
        {
            if (powerMaterial)
            {
                powerMaterial.color = offColor;
            }

            for (int i = 0; i < instrumentButton.Count; i++)
            {
                instrumentButton[i].OnMouseButtonClickEvent += instrumentButtonFunc;
            }
        }

        /// <summary>
        /// 输出设备上的按钮
        /// </summary>
        /// <param name="buttonName"></param>
        public void instrumentButtonFunc(string buttonName)
        {
            Debug.Log(buttonName + "-------------");
            InstrumentButton tempInstrumentBtn = instrumentButton.Find(x => x.instrumentButton.name == buttonName);
            if (tempInstrumentBtn != null)
            {
                TempMaxStep = tempInstrumentBtn.StepLength;
                switch (buttonName.Trim())
                {
                    #region 微波信号源
                    case "SquareWaveBtn":
                        break;
                    case "EqualAmplitudeBtn":
                        break;
                    case "PowerBtn":
                        {
                            if (MathUtility.GetCurrentValue(tempInstrumentBtn) == 0)
                            {
                                powerMaterial.color = onColor;
                            }
                            else
                            {
                                powerMaterial.color = offColor;
                            }
                        }
                        break;
                    case "ElectricCurrentBtn":
                        break;
                    case "VoltageBtn":
                        break;
                    case "FrequencyBtn":
                        break;
                    #endregion
                    #region 频选放大器
                    case "RangeBtnX10":
                        break;
                    case "RangeBtnX1":
                        break;
                    case "RangeBtnX01":
                        break;
                    case "VoltageBtnX1":
                        break;
                    case "VoltageBtnX10":
                        break;
                    case "VoltageBtnX100":
                        break;
                    case "VoltageBtnX1000":
                        break;
                    case "FrequencyBtnKuanDai":
                        break;
                    case "FrequencyBtn1K":
                        break;
                    case "FrequencyBtn2K":
                        break;
                    case "FrequencyBtn5K":
                        break;
                    case "FrequencySelectiveAmplifierPowerBtn":
                        {
                            if (MathUtility.GetCurrentValue(tempInstrumentBtn) == 0)
                            {
                                powerMaterial.color = onColor;
                            }
                            else
                            {
                                powerMaterial.color = offColor;
                            }
                        }
                        break;
                    case "RotaryBtnVoltage":
                        //SetStepLength(instrumentButton.Find(x => x.instrumentButton.name == "RotaryBtnVoltage") == null ? 0 : instrumentButton.Find(x => x.instrumentButton.name == "RotaryBtnVoltage").rotary);
                        break;
                    case "RotaryBtnFrequency":
                        break;
                    case "RotaryBtnGain":
                        break;
                    case "RotaryBtnWithered":
                        break;
                    #endregion
                    #region 三厘米测量线
                    case "FrequencySelectKnob":
                        Debug.Log("三厘米测量线 游标");
                        ToggleTheKnobToMoveTheCursor(tempInstrumentBtn);
                        break;
                    #endregion
                    #region 二端口网络调节
                    case "Gear":
                        Debug.Log("二端口网络调节 尺子");
                        ToggleSwitchAdjustmentRuler(tempInstrumentBtn);
                        break;
                    #endregion
                    #region 可变短路器
                    case "kebianduanluqi4":
                        Debug.Log("可变短路器 尺子");
                        RotaryVariableCircuitBreaker(tempInstrumentBtn);
                        break;
                    #endregion
                    #region 可变衰减器
                    case "Kebianshaijianqi":
                        Debug.Log("可变衰减器 旋钮");
                        VariableAttenuatorRatary(tempInstrumentBtn);
                        break;
                    #endregion
                    #region 匹配螺钉
                    case "PPLDGear":
                        MatchingScrewActive(tempInstrumentBtn);
                        break;
                    case "PiPeiLuoDingBtn":
                        MatchingScrewHorMove(tempInstrumentBtn);
                        break;
                    #endregion
                    #region 支架
                    case "zhijiaBtn":
                        BracketVecMoving(tempInstrumentBtn);
                        break;

                    #endregion
                    default:
                        Debug.LogWarning("出现名称不符合，请查找名称:" + buttonName);
                        break;
                }
                Debug.Log(buttonName.Trim() + "仪器的当前值为" + MathUtility.GetCurrentValue(tempInstrumentBtn));
            }
        }

        #region 支架
        private void BracketVecMoving(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = tempInstrumentBtn.rotary * GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "zhijiagan");
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempInstrumentBtn.StartMovePoint + offset, tempTransform.localPosition.z);
            }

        }
        #endregion
        #region 匹配螺钉

        private void MatchingScrewHorMove(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = tempInstrumentBtn.rotary * GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Body");
                tempTransform.localPosition = new Vector3(tempInstrumentBtn.StartMovePoint + offset, tempTransform.localPosition.y, tempTransform.localPosition.z);
            }
        }


        private void MatchingScrewActive(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = tempInstrumentBtn.rotary * GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "PPLDGear");
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempInstrumentBtn.StartMovePoint + offset, tempTransform.localPosition.z);
            }
        }

        #endregion


        #region 可变衰减器
        private void VariableAttenuatorRatary(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = tempInstrumentBtn.rotary * GetPerStepMoveDistance(tempInstrumentBtn);
                Debug.Log(offset);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Kebianshaijianqi");
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempInstrumentBtn.StartMovePoint + offset, tempTransform.localPosition.z);
            }
        }
        #endregion

        #region 二端口网络调节
        private void ToggleSwitchAdjustmentRuler(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = tempInstrumentBtn.rotary * GetPerStepMoveDistance(tempInstrumentBtn);
                Debug.Log(offset);
                //
                Transform ruler1Transform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Ruler1");
                ruler1Transform.localPosition = new Vector3(-(tempInstrumentBtn.StartMovePoint) - offset, ruler1Transform.localPosition.y, ruler1Transform.localPosition.z);
                Transform ruler2Transform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Ruler2");
                ruler2Transform.localPosition = new Vector3(tempInstrumentBtn.StartMovePoint + offset, ruler2Transform.localPosition.y, ruler1Transform.localPosition.z);
            }
        }

        #endregion

        #region 三厘米测量线


        /// <summary>
        /// 获取每步行进距离
        /// </summary>
        /// <param name="tempInstrumentBtn"></param>
        /// <returns></returns>
        private float GetPerStepMoveDistance(InstrumentButton tempInstrumentBtn)
        {
            return (tempInstrumentBtn.EndMovePoint - tempInstrumentBtn.StartMovePoint) / tempInstrumentBtn.StepLength;
        }


        //移动动画
        private void ToggleTheKnobToMoveTheCursor(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                //拿到旋钮 移动刻度值
                float offset = tempInstrumentBtn.rotary * GetPerStepMoveDistance(tempInstrumentBtn);
                Debug.Log(offset);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Cursor");
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.StartMovePoint + offset);
            }
        }

        #endregion
        #region 可变短路器
        private void RotaryVariableCircuitBreaker(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = tempInstrumentBtn.rotary * GetPerStepMoveDistance(tempInstrumentBtn);
                Debug.Log(offset);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "kebianduanluqi4");
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.StartMovePoint + offset);
            }

        }

        #endregion
        [Header("InstrumentButton Setting")]
        public List<InstrumentButton> instrumentButton = new List<InstrumentButton>();
        private void FixedUpdate()
        {
            PointerRotate();
        }















    }
}