using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static DLKJ.InstrumentAction.InstrumentButton;

namespace DLKJ
{
    public partial class InstrumentAction : MonoBehaviour
    {
        [Header("Camera Displayer")]
        [SerializeField] public Camera voltmeterCamera;

        [Header("Pointer Settings")]
        //  public Transform pointer;
        public Pointer pointer;//ָ��
        [Header("Power Material")]
        [SerializeField] List<Material> powerMaterials = new List<Material>();
        [SerializeField] Color onColor;
        [SerializeField] Color offColor;

        [Header("Numbers Bits")]
        [SerializeField] List<MeshRenderer> numbers = new List<MeshRenderer>();
        [Header("Frequency")]
        [SerializeField] List<Texture2D> numberTextures = new List<Texture2D>();

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
        public PressButtonGroup group;
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
                //switch (rotationType)
                //{
                //    case RotationType.Y_AxisRotation:
                //        pointer.localRotation = Quaternion.Lerp(pointer.localRotation, Quaternion.Euler(Vector3.up * (ReturnAngle(StepLength) % 360)), Time.deltaTime * RotationSpeed);
                //        break;
                //    case RotationType.X_AxisRotation:
                //        pointer.localRotation = Quaternion.Lerp(pointer.localRotation, Quaternion.Euler(Vector3.right * (ReturnAngle(StepLength) % 360)), Time.deltaTime * RotationSpeed);
                //        break;
                //    case RotationType.Z_AxisRotation:
                //        pointer.localRotation = Quaternion.Lerp(pointer.localRotation, Quaternion.Euler(Vector3.forward * (ReturnAngle(StepLength) % 360)), Time.deltaTime * RotationSpeed);
                //        break;
                //    default:
                //        break;
                //}
            }
        }

        private MaterialPropertyBlock mpb;



        private void Awake()
        {
            mpb = new MaterialPropertyBlock();
            for (int i = 0; i < powerMaterials.Count; i++)
            {
                powerMaterials[i].color = offColor;
            }

            for (int i = 0; i < instrumentButton.Count; i++)
            {
                instrumentButton[i].OnMouseButtonClickEvent += instrumentButtonFunc;
                ///��ʼ���Ƕ�
                //switch (instrumentButton[i].btnRotationType)
                //{
                //    case BtnRotationType.X_AxisRotation:
                //        instrumentButton[i].rotary = instrumentButton[i].instrumentButton.localEulerAngles.x;
                //        break;
                //    case BtnRotationType.Y_AxisRotation:
                //        instrumentButton[i].rotary = instrumentButton[i].instrumentButton.localEulerAngles.y;
                //        break;
                //    case BtnRotationType.Z_AxisRotation:
                //        instrumentButton[i].rotary = instrumentButton[i].instrumentButton.localEulerAngles.z;
                //        break;
                //    default:
                //        break;
                //}

            }
            pointer = GetComponentInChildren<Pointer>();
            mpb = new MaterialPropertyBlock();

            if (voltmeterCamera)
            {
                //  voltmeterCamera.targetTexture = UIManager.GetInstance().voltmeterValue
            }
        }

        /// <summary>
        /// ����豸�ϵİ�ť
        /// </summary>
        /// <param name="buttonName"></param>
        public void instrumentButtonFunc(string buttonName)
        {
            //     Debug.Log(buttonName + "-------------");
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
                                //У���Ƿ���ȷ
                                if (SceneManager.GetInstance().CurrentStepVerify())
                                {
                                    MathTest.Instance.FormulaInit();
                                    for (int i = 0; i < powerMaterials.Count; i++)
                                    {
                                        powerMaterials[i].color = onColor;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < powerMaterials.Count; i++)
                                {
                                    powerMaterials[i].color = offColor;
                                }
                                MathTest.Instance.Active(false);
                                if (pointer != null)
                                    pointer.SetAngle(0);
                            }
                        }

                        break;
                    case "ElectricCurrentBtn":
                        break;
                    case "VoltageBtn":
                        break;
                    case "FrequencyBtn":
                        if (MathTest.Instance.isInit == false) return;
                        //��Ƶ�ʸ�ֵ
                        MathTool.F = MathUtility.GetCurrentValue(tempInstrumentBtn);
                        MathTest.Instance.mathInitValue.initF = true;
                        UIManager.GetInstance().SetStartButton();
                        UpdateNumber(MathTool.F);
                        //  MathTool.F = MathUtility.GetCurrentValue(tempInstrumentBtn);
                        Debug.Log(MathTool.F);
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
                                for (int i = 0; i < powerMaterials.Count; i++)
                                {
                                    powerMaterials[i].color = onColor;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < powerMaterials.Count; i++)
                                {
                                    powerMaterials[i].color = offColor;
                                }
                            }
                        }
                        break;
                    case "RotaryBtnVoltage":
                        if (MathTest.Instance.isInit == false)
                            return;
                        //����ѹ��ֵ
                        //   MathTool.A = MathUtility.GetCurrentValue(tempInstrumentBtn);
                        // UIManager.GetInstance().SetStartButton();
                        //transform.Find("��ѹText").GetComponent<TextMesh>().text = MathTool.A.ToString("#0.00");
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
                        MathTool.distanceZ = MathUtility.GetDistance(tempInstrumentBtn);
                        ToggleTheKnobToMoveTheCursor(tempInstrumentBtn);
                        UIManager.GetInstance().PinXuanView();
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
                        UIManager.GetInstance().PinXuanView();
                        break;
                    #endregion
                    #region �ɱ�˥����
                    case "Kebianshaijianqi":
                        Debug.Log("�ɱ�˥���� ��ť");
                        MathTool.�� = MathUtility.GetCurrentValue(tempInstrumentBtn);
                        MathTest.Instance.mathInitValue.init�� = true;
                        UIManager.GetInstance().SetStartButton();
                        VariableAttenuatorRatary(tempInstrumentBtn);
                        UIManager.GetInstance().PinXuanView();
                        break;
                    #endregion
                    #region ƥ���ݶ�
                    case "PPLDGear":
                        UIManager.GetInstance().PinXuanView();
                        MatchingScrewActive(tempInstrumentBtn);
                        break;
                    case "PiPeiLuoDingBtn":
                        UIManager.GetInstance().PinXuanView();
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
                float offset = GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Body");
                if (tempTransform.localPosition.x + offset > tempInstrumentBtn.EndMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempInstrumentBtn.EndMovePoint, tempTransform.localPosition.y, tempTransform.localPosition.z);
                    return;
                }
                if (tempTransform.localPosition.x + offset < tempInstrumentBtn.StartMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempInstrumentBtn.StartMovePoint, tempTransform.localPosition.y, tempTransform.localPosition.z);
                    return;
                }
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x + offset, tempTransform.localPosition.y, tempTransform.localPosition.z);
            }
        }


        private void MatchingScrewActive(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = /*tempInstrumentBtn.rotary * */GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "PPLDGear");
                if (tempTransform.localPosition.z + offset < tempInstrumentBtn.EndMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.EndMovePoint);
                    return;
                }
                if (tempTransform.localPosition.z + offset > tempInstrumentBtn.StartMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.StartMovePoint);
                    return;
                }
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempTransform.localPosition.z + offset);
            }
        }

        #endregion


        #region �ɱ�˥����
        private void VariableAttenuatorRatary(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.CanInteractive == false) return;
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Kebianshaijianqi");
                float offset = GetPerStepMoveDistance(tempInstrumentBtn);
                if (tempTransform.localPosition.y + offset < tempInstrumentBtn.EndMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempInstrumentBtn.EndMovePoint, tempTransform.localPosition.z);
                    return;
                }
                if (tempTransform.localPosition.y + offset > tempInstrumentBtn.StartMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempInstrumentBtn.StartMovePoint, tempTransform.localPosition.z);
                    return;
                }
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y + offset, tempTransform.localPosition.z);
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
            float result = (tempInstrumentBtn.EndMovePoint - tempInstrumentBtn.StartMovePoint) / tempInstrumentBtn.StepLength;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                result = -result;
            }
            return result;
        }


        //�ƶ�����
        private void ToggleTheKnobToMoveTheCursor(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                //�õ���ť �ƶ��̶�ֵ
                float offset = /*tempInstrumentBtn.rotary **/ GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Cursor");
                if (tempTransform.localPosition.z + offset < tempInstrumentBtn.EndMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.EndMovePoint);
                    return;
                }
                if (tempTransform.localPosition.z + offset > tempInstrumentBtn.StartMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.StartMovePoint);
                    return;
                }
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempTransform.localPosition.z + offset);

            }
        }

        #endregion
        #region �ɱ��·��
        private void RotaryVariableCircuitBreaker(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "kebianduanluqi4");
                if (tempTransform.localPosition.z + offset > tempInstrumentBtn.EndMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.EndMovePoint);
                    return;
                }
                if (tempTransform.localPosition.z + offset < tempInstrumentBtn.StartMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.StartMovePoint);
                    return;
                }
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempTransform.localPosition.z + offset);
            }

        }

        #endregion
        [Header("InstrumentButton Setting")]
        public List<InstrumentButton> instrumentButton = new List<InstrumentButton>();
        private void FixedUpdate()
        {
            PointerRotate();
        }

        private void Update()
        {
            if (pointer != null)
                pointer.PointerRotate();
            if (voltmeterCamera)
            {
                UIManager.GetInstance().SetVoltmeterValue(voltmeterCamera);
            }
        }


        public void UpdateNumber(float frequencyValue)
        {
            Debug.Log("Ƶ��ֵ:" + frequencyValue);
            int value0 = 0;
            int value1 = 0;
            int value2 = 0;
            int value3 = 0;

            if (frequencyValue > 0)
            {
                float value = frequencyValue;

                if (frequencyValue < 1)
                {
                    value = value * 100;
                    string vslueStr = value.ToString();
                    string[] arry = vslueStr.Split('.');

                    string first = arry[0].Substring(0, 1);
                    string second = arry[0].Substring(1, 1);
                    //Debug.Log("first:" + first + "   second:" + second);

                    value0 = 0;
                    value1 = 0;
                    value2 = int.Parse(first);
                    value3 = int.Parse(second);
                }
                else if (1 <= frequencyValue && frequencyValue < 10)
                {
                    value = value * 100;
                    string vslueStr = value.ToString();
                    string[] arry = vslueStr.Split('.');

                    string first = arry[0].Substring(0, 1);
                    string second = arry[0].Substring(1, 1);
                    string third = arry[0].Substring(2, 1);
                    //Debug.Log("first:" + first + "   second:" + second+ "    third:" + third);

                    value0 = 0;
                    value1 = int.Parse(first);
                    value2 = int.Parse(second);
                    value3 = int.Parse(third);
                }
                else if (10 <= frequencyValue && frequencyValue < 100)
                {
                    value = value * 100;
                    string vslueStr = value.ToString();
                    string[] arry = vslueStr.Split('.');

                    string first = arry[0].Substring(0, 1);
                    string second = arry[0].Substring(1, 1);
                    string third = arry[0].Substring(2, 1);
                    string forth = arry[0].Substring(3, 1);
                    //Debug.Log("first:" + first + "   second:" + second+ "    third:" + third+ "  forth:" + forth);

                    value0 = int.Parse(first);
                    value1 = int.Parse(second);
                    value2 = int.Parse(third);
                    value3 = int.Parse(forth);
                }

                for (int i = 0; i < numbers.Count; i++)
                {
                    numbers[i].GetPropertyBlock(mpb);
                    switch (i)
                    {
                        case 0:
                            {
                                mpb.SetTexture("_MainTex", numberTextures[value0]);
                            }
                            break;
                        case 1:
                            {
                                mpb.SetTexture("_MainTex", numberTextures[value1]);
                            }
                            break;
                        case 2:
                            {
                                mpb.SetTexture("_MainTex", numberTextures[value2]);
                            }
                            break;
                        case 3:
                            {
                                mpb.SetTexture("_MainTex", numberTextures[value3]);
                            }
                            break;
                        default:
                            break;
                    }
                    numbers[i].SetPropertyBlock(mpb);
                }
            }
            else
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    numbers[i].GetPropertyBlock(mpb);
                    mpb.SetTexture("_MainTex", numberTextures[0]);
                    numbers[i].SetPropertyBlock(mpb);
                }
            }
        }

    }
}