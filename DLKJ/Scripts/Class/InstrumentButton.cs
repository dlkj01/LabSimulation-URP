using System;
using System.Collections.Generic;
using UnityEngine;
namespace DLKJ
{
    public partial class InstrumentAction
    {

        [Serializable]
        public class InstrumentButton
        {
            public bool CanInteractive = true;
            public enum ClickBtnState { Press, Lift };
            public enum BtnRotationType { Y_AxisRotation, X_AxisRotation, Z_AxisRotation }
            public enum InstrumentButtonType { Power, Rotary, Click, Translate };
            public Transform instrumentButton;
            public List<Transform> conbinationList = new List<Transform>();
            public ClickBtnState clickState;
            public BtnRotationType btnRotationType;
            public InstrumentButtonType instrumentButtonType;
            public float rotary = 0f;
            public float rotatyDirection = 1f;
            /// <summary>
            /// 变量最大值
            /// </summary>
            public float MaxValue = 0;
            /// <summary>
            /// 变量最小值
            /// </summary>
            public float MinValue = 0;
            /// <summary>
            /// 仪器运动终点
            /// </summary>
            public float EndMovePoint = 0;
            /// <summary>
            /// 仪器运动起点
            /// </summary>
            public float StartMovePoint = 0;
            /// <summary>
            /// 最大移动步长
            /// </summary>
            public float StepLength = 0;

            /// <summary>
            /// 最小旋转角度
            /// </summary>
            public float StartAngle;
            /// <summary>
            /// 最大旋转角度
            /// </summary>
            public float EndAngle;

            public float currentAngle = 0;

            public bool uncontrolled;//点击不受控制

            public delegate void OnMouseButtonClick(string buttonName);

            public event OnMouseButtonClick OnMouseButtonClickEvent;

            public void OnMouseClick()
            {
                OnMouseButtonClickEvent(instrumentButton.name);
            }

            public InstrumentButton()
            {
                OnMouseButtonClickEvent += ClickState;
            }
            public void AddListener()
            {
                OnMouseButtonClickEvent += ClickState;
            }
            public void RemoveListener()
            {
                OnMouseButtonClickEvent -= ClickState;
            }
            public void SetInteractiveState(bool state)
            {
                CanInteractive = state;
            }
            public void ClickState(string buttonName)
            {
                switch (clickState)
                {
                    case ClickBtnState.Press:
                        ClickPressAction();
                        break;
                    case ClickBtnState.Lift:
                        ClickLiftAction();
                        break;
                    default:
                        break;
                }
            }
            public float x;
            private void ClickPressAction()
            {
                clickState = ClickBtnState.Lift;
                switch (instrumentButtonType)
                {
                    case InstrumentButtonType.Power:
                        instrumentButton.localRotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case InstrumentButtonType.Rotary:
                        if (instrumentButton.name == "FrequencyBtn" || instrumentButton.name == "RotaryBtnVoltage")
                            if (MathTest.Instance.isOpen == false) return;
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            if (Mathf.Approximately(currentAngle, StartAngle) || currentAngle - 0.001f <= StartAngle)
                            {
                                currentAngle = StartAngle;
                                InstrumentButtonTypeSwitch(btnRotationType);
                                return;
                            }

                            currentAngle -= (EndAngle - StartAngle) / StepLength /*rotatyDirection*/;
                            InstrumentButtonTypeSwitch(btnRotationType);
                        }
                        else
                        {
                            if (Mathf.Approximately(currentAngle, EndAngle) || currentAngle + 0.001f >= EndAngle)
                            {
                                currentAngle = EndAngle;
                                InstrumentButtonTypeSwitch(btnRotationType);
                                return;
                            }
                            currentAngle += (EndAngle - StartAngle) / StepLength;
                            InstrumentButtonTypeSwitch(btnRotationType);
                        }
                        break;
                    case InstrumentButtonType.Click:
                        //如果是频选放大器的开关,需要检测当前连接是否正确
                        if (instrumentButton.name == "PowerBtn")
                        {
                            instrumentButton.localPosition = new Vector3(x,
                                                               instrumentButton.localPosition.y,
                                                               instrumentButton.localPosition.z);
                            return;
                        }
                        if (uncontrolled) return;
                        instrumentButton.localPosition = new Vector3(instrumentButton.localPosition.x + instrumentButton.GetComponent<MeshCollider>().bounds.size.x / 3,
                                                                 instrumentButton.localPosition.y,
                                                                 instrumentButton.localPosition.z);
                        break;
                    default:
                        break;
                }

            }

            private void InstrumentButtonTypeSwitch(BtnRotationType btnRotationType)
            {
                switch (btnRotationType)
                {
                    case BtnRotationType.Y_AxisRotation:
                        instrumentButton.localRotation = Quaternion.Euler(0, currentAngle * rotatyDirection  /** 3.6f*/, 0);
                        break;
                    case BtnRotationType.X_AxisRotation:
                        instrumentButton.localRotation = Quaternion.Euler(currentAngle * rotatyDirection  /** 3.6f*/, 0, 0);
                        break;
                    case BtnRotationType.Z_AxisRotation:
                        instrumentButton.localRotation = Quaternion.Euler(0, 0, currentAngle * rotatyDirection  /** 3.6f*/);
                        break;
                    default:
                        Debug.Log("数据出现异常");
                        break;
                }
            }

            private void ClickLiftAction()
            {

                clickState = ClickBtnState.Press;
                switch (instrumentButtonType)
                {
                    case InstrumentButtonType.Power:

                        instrumentButton.localRotation = Quaternion.Euler(0, 0, -25);
                        break;
                    case InstrumentButtonType.Rotary:
                        if (instrumentButton.name == "FrequencyBtn" || instrumentButton.name == "RotaryBtnVoltage")
                            if (MathTest.Instance.isOpen == false) return;
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            if (Mathf.Approximately(currentAngle, StartAngle) || currentAngle - 0.001f <= StartAngle)
                            {
                                currentAngle = StartAngle;
                                InstrumentButtonTypeSwitch(btnRotationType);
                                return;
                            }
                            currentAngle -= (EndAngle - StartAngle) / StepLength /*rotatyDirection*/;
                            InstrumentButtonTypeSwitch(btnRotationType);
                        }
                        else
                        {
                            if (Mathf.Approximately(currentAngle, EndAngle) || currentAngle + 0.001f >= EndAngle)
                            {
                                currentAngle = EndAngle;
                                InstrumentButtonTypeSwitch(btnRotationType);
                                return;
                            }
                            currentAngle += (EndAngle - StartAngle) / StepLength /*rotatyDirection*/;
                            InstrumentButtonTypeSwitch(btnRotationType);
                        }
                        break;
                    case InstrumentButtonType.Click:
                        if (instrumentButton.name == "PowerBtn")
                            if (!SceneManager.GetInstance().VerifyBasicLink()) return;
                        if (uncontrolled) return;
                        instrumentButton.localPosition = new Vector3(instrumentButton.localPosition.x - instrumentButton.GetComponent<MeshCollider>().bounds.size.x / 3,
                                                                     instrumentButton.localPosition.y,
                                                                     instrumentButton.localPosition.z);
                        break;
                    default:
                        break;
                }
            }

        }

    }
}