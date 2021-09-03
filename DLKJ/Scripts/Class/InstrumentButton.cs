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
            public enum ClickBtnState { Press, Lift };
            public enum BtnRotationType { Y_AxisRotation, X_AxisRotation, Z_AxisRotation }
            public enum InstrumentButtonType { Power, Rotary, Click };
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
            private void ClickPressAction()
            {
                clickState = ClickBtnState.Lift;
                switch (instrumentButtonType)
                {
                    case InstrumentButtonType.Power:
                        instrumentButton.localRotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case InstrumentButtonType.Rotary:

                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            if (rotary <= 0)
                            {
                                return;
                            }
                            rotary -= 1;
                            InstrumentButtonTypeSwitch(btnRotationType);
                        }
                        else
                        {
                            if (rotary >= StepLength)
                            {
                                return;
                            }
                            rotary += 1;
                            InstrumentButtonTypeSwitch(btnRotationType);
                        }
                        break;
                    case InstrumentButtonType.Click:
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
                        instrumentButton.localRotation = Quaternion.Euler(0, rotatyDirection * rotary * 3.6f, 0);
                        break;
                    case BtnRotationType.X_AxisRotation:
                        instrumentButton.localRotation = Quaternion.Euler(rotatyDirection * rotary * 3.6f, 0, 0);
                        break;
                    case BtnRotationType.Z_AxisRotation:
                        instrumentButton.localRotation = Quaternion.Euler(0, 0, rotatyDirection * rotary * 3.6f);
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
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            if (rotary <= 0)
                            {
                                return;
                            }
                            rotary -= 1;
                            InstrumentButtonTypeSwitch(btnRotationType);
                        }
                        else
                        {
                            if (rotary >= 100)
                            {
                                return;
                            }
                            rotary += 1;
                            InstrumentButtonTypeSwitch(btnRotationType);
                        }
                        break;
                    case InstrumentButtonType.Click:
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