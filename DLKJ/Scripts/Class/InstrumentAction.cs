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
        /// ģ����ת�ٶ�
        /// </summary>
        public float RotationSpeed = 2;
        /// <summary>
        /// ��ǰ����
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
        /// ������ֵ���ؽǶ�
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float ReturnAngle(float value)
        {
            return -45f + value * 0.9f;
        }
        /// <summary>
        /// ���ò���
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
        /// ����豸�ϵİ�ť
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
                    #region ΢���ź�Դ
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
                    #region Ƶѡ�Ŵ���
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
                    #region �����ײ�����
                    case "FrequencySelectKnob":
                        Debug.Log("�����ײ����� �α�");
                        ToggleTheKnobToMoveTheCursor(tempInstrumentBtn);
                        break;
                    #endregion
                    #region ���˿��������
                    case "Gear":
                        Debug.Log("���˿�������� ����");
                        ToggleSwitchAdjustmentRuler(tempInstrumentBtn);
                        break;
                    #endregion
                    #region �ɱ��·��
                    case "kebianduanluqi4":
                        Debug.Log("�ɱ��·�� ����");
                        RotaryVariableCircuitBreaker(tempInstrumentBtn);
                        break;
                    #endregion
                    #region �ɱ�˥����
                    case "Kebianshaijianqi":
                        Debug.Log("�ɱ�˥���� ��ť");
                        VariableAttenuatorRatary(tempInstrumentBtn);
                        break;
                    #endregion
                    #region ƥ���ݶ�
                    case "PPLDGear":
                        MatchingScrewActive(tempInstrumentBtn);
                        break;
                    case "PiPeiLuoDingBtn":
                        MatchingScrewHorMove(tempInstrumentBtn);
                        break;
                    #endregion
                    #region ֧��
                    case "zhijiaBtn":
                        BracketVecMoving(tempInstrumentBtn);
                        break;

                    #endregion
                    default:
                        Debug.LogWarning("�������Ʋ����ϣ����������:" + buttonName);
                        break;
                }
                Debug.Log(buttonName.Trim() + "�����ĵ�ǰֵΪ" + MathUtility.GetCurrentValue(tempInstrumentBtn));
            }
        }

        #region ֧��
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
        #region ƥ���ݶ�

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


        #region �ɱ�˥����
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

        #region ���˿��������
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

        #region �����ײ�����


        /// <summary>
        /// ��ȡÿ���н�����
        /// </summary>
        /// <param name="tempInstrumentBtn"></param>
        /// <returns></returns>
        private float GetPerStepMoveDistance(InstrumentButton tempInstrumentBtn)
        {
            return (tempInstrumentBtn.EndMovePoint - tempInstrumentBtn.StartMovePoint) / tempInstrumentBtn.StepLength;
        }


        //�ƶ�����
        private void ToggleTheKnobToMoveTheCursor(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                //�õ���ť �ƶ��̶�ֵ
                float offset = tempInstrumentBtn.rotary * GetPerStepMoveDistance(tempInstrumentBtn);
                Debug.Log(offset);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Cursor");
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.StartMovePoint + offset);
            }
        }

        #endregion
        #region �ɱ��·��
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