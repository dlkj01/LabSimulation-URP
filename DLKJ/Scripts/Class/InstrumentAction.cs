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
        public Pointer pointer;//指针
        [Header("Power Material")]
        [SerializeField] List<Material> powerMaterials = new List<Material>();
        [SerializeField] Color onColor;
        [SerializeField] Color offColor;

        [Header("Numbers Bits Of GHZ")]
        [SerializeField] List<MeshRenderer> numbers = new List<MeshRenderer>();
        [Header("Numbers Bits Of UV")]
        [SerializeField] List<MeshRenderer> numbersUV = new List<MeshRenderer>();
        [Header("Frequency")]
        [SerializeField] List<Texture2D> numberTextures = new List<Texture2D>();

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
                ///初始化角度
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
                voltmeterCamera.gameObject.SetActive(false);
                //  voltmeterCamera.targetTexture = UIManager.GetInstance().voltmeterValue
            }
        }

        /// <summary>
        /// 输出设备上的按钮
        /// </summary>
        /// <param name="buttonName"></param>
        public void instrumentButtonFunc(string buttonName)
        {
            //     Debug.Log(buttonName + "-------------");
            InstrumentButton tempInstrumentBtn = instrumentButton.Find(x => x.instrumentButton.name == buttonName);
            InstrumentButton powerKaiGuan = SceneManager.GetInstance().GetInstrumentButton("微波信号源", "PowerBtn");
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
                                //校验是否正确
                                if (SceneManager.GetInstance().CurrentStepVerify() && CanSetValue())
                                {
                                    MathTest.Instance.FormulaInit();
                                    
                                    InstrumentButton buttonKaiGuan = SceneManager.GetInstance().GetInstrumentButton("选频放大器", "FrequencySelectiveAmplifierPowerBtn");
                                    if (MathUtility.GetCurrentValue(buttonKaiGuan) == 0 && MathUtility.GetCurrentValue(powerKaiGuan) == 0)
                                    {
                                        MathTest.Instance.IsOpen(true);
                                        Transform light = buttonKaiGuan.instrumentButton.transform.parent.Find("pSphere2");
                                        light.GetComponent<MeshRenderer>().materials[0].color = Color.red;

                                    }
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
                                MathTest.Instance.IsOpen(false);
                                Item itemXuanPin = SceneManager.GetInstance().GetItemByName("选频放大器");
                                itemXuanPin.GetComponentInChildren<Pointer>().SetAngle(0);
                                Transform light = itemXuanPin.transform.Find("pSphere2");
                                light.GetComponent<MeshRenderer>().materials[0].color = Color.black;
                            }
                        }

                        break;
                    case "ElectricCurrentBtn":
                        break;
                    case "VoltageBtn":
                        break;
                    case "FrequencyBtn":
                        if (!CanSetValue() || !SceneManager.GetInstance().CurrentStepVerify())
                            return;
                        if (MathUtility.GetCurrentValue(powerKaiGuan) != 0)
                        {
                            return;
                        }
                        //给频率赋值
                        MathTool.F = MathUtility.GetCurrentValue(tempInstrumentBtn);
                        MathTest.Instance.mathInitValue.initF = true;
                        UIManager.GetInstance().SetStartButton();
                        UpdateHZNumber(MathTool.F);
                        Debug.Log(MathTool.F);
                        break;
                    case "FrequencyBtn2":
                        if (!CanSetValue() || !SceneManager.GetInstance().CurrentStepVerify())
                            return;
                        if (MathUtility.GetCurrentValue(powerKaiGuan) != 0)
                        {
                            return;
                        }
                        // 给电压赋值
                        MathTool.A = MathUtility.GetCurrentValue(tempInstrumentBtn);
                        MathTest.Instance.mathInitValue.initA = true;
                        UIManager.GetInstance().SetStartButton();
                        UpdateUVNumber(MathTool.A);

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
                                //校验是否正确
                                if (SceneManager.GetInstance().CurrentStepVerify())
                                {
                                    Transform light = tempInstrumentBtn.instrumentButton.transform.parent.Find("pSphere2");
                                    light.GetComponent<MeshRenderer>().materials[0].color = Color.red;
                                    for (int i = 0; i < powerMaterials.Count; i++)
                                    {
                                        powerMaterials[i].color = onColor;
                                    }
                                    InstrumentButton buttonKaiGuan = SceneManager.GetInstance().GetInstrumentButton("选频放大器", "FrequencySelectiveAmplifierPowerBtn");
                                    if (MathUtility.GetCurrentValue(buttonKaiGuan) == 0 && MathUtility.GetCurrentValue(powerKaiGuan) == 0)
                                    {
                                        MathTest.Instance.IsOpen(true);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < powerMaterials.Count; i++)
                                {
                                    powerMaterials[i].color = offColor;
                                }
                                MathTest.Instance.IsOpen(false);
                                Transform light = tempInstrumentBtn.instrumentButton.transform.parent.Find("pSphere2");
                                light.GetComponent<MeshRenderer>().materials[0].color = Color.black;
                                Item itemXuanPin = SceneManager.GetInstance().GetItemByName("选频放大器");
                                itemXuanPin.GetComponentInChildren<Pointer>().SetAngle(0);
                            }
                        }
                        break;
                    case "RotaryBtnVoltage":

                        //transform.Find("电压Text").GetComponent<TextMesh>().text = MathTool.A.ToString("#0.00");
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
                        MathTool.distanceZ = MathUtility.GetDistance(tempInstrumentBtn);
                        ToggleTheKnobToMoveTheCursor(tempInstrumentBtn);
                        UIManager.GetInstance().PinXuanView();
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
                        UIManager.GetInstance().PinXuanView();
                        break;
                    #endregion
                    #region 可变衰减器
                    case "Kebianshaijianqi":
                        Debug.Log("可变衰减器 旋钮");
                        MathTool.δ = MathUtility.GetCurrentValue(tempInstrumentBtn);
                        MathTest.Instance.mathInitValue.initδ = true;
                        UIManager.GetInstance().SetStartButton();
                        VariableAttenuatorRatary(tempInstrumentBtn);
                        UIManager.GetInstance().PinXuanView();
                        break;
                    #endregion
                    #region 匹配螺钉
                    case "PPLDGear":
                        UIManager.GetInstance().PinXuanView();
                        MatchingScrewActive(tempInstrumentBtn);
                        break;
                    case "PiPeiLuoDingBtn":
                        UIManager.GetInstance().PinXuanView();
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
                //Debug.Log(buttonName.Trim() + "仪器的当前值为" + MathUtility.GetCurrentValue(tempInstrumentBtn));
            }
        }

        private bool CanSetValue()
        {
            bool canNext = true;
            switch (SceneManager.GetInstance().GetCurrentLabName())
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    if (SceneManager.GetInstance().GetCurrentStep() < 2)
                        canNext = false;
                    break;
                default:
                    break;
            }
            return canNext;
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
                float offset = GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "Body");
                if (tempTransform.localPosition.z + offset < tempInstrumentBtn.StartMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.StartMovePoint);
                    return;
                }
                if (tempTransform.localPosition.z + offset > tempInstrumentBtn.EndMovePoint)
                {
                    tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempInstrumentBtn.EndMovePoint);
                    return;
                }
                tempTransform.localPosition = new Vector3(tempTransform.localPosition.x, tempTransform.localPosition.y, tempTransform.localPosition.z + offset);
            }
        }


        private void MatchingScrewActive(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                float offset = /*tempInstrumentBtn.rotary * */GetPerStepMoveDistance(tempInstrumentBtn);
                Transform tempTransform = tempInstrumentBtn.conbinationList.Find(x => x.name == "PPLDGear");
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


        #region 可变衰减器
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
            float result = (tempInstrumentBtn.EndMovePoint - tempInstrumentBtn.StartMovePoint) / tempInstrumentBtn.StepLength;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                result = -result;
            }
            return result;
        }


        //移动动画
        private void ToggleTheKnobToMoveTheCursor(InstrumentButton tempInstrumentBtn)
        {
            if (tempInstrumentBtn.conbinationList.Count > 0)
            {
                //拿到旋钮 移动刻度值
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
        #region 可变短路器
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


        public void UpdateHZNumber(float frequencyValue)
        {
            Debug.Log("频率值:" + frequencyValue);
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
        public void UpdateUVNumber(float voltageValue)
        {
            Debug.Log("电压值:" + voltageValue);
            int value0 = 0;
            int value1 = 0;
            int value2 = 0;
            int value3 = 0;
            int value4 = 0;


            if (voltageValue > 0)
            {
                float value = voltageValue;

                if (voltageValue < 1)
                {
                    value = value * 100;
                    string vslueStr = value.ToString();
                    string[] arry = vslueStr.Split('.');

                    string first = arry[0].Substring(0, 1);
                    string second = arry[0].Substring(1, 1);
                    //Debug.Log("first:" + first + "   second:" + second);

                    value0 = 0;
                    value1 = 0;
                    value2 = 0;
                    value3 = int.Parse(first);
                    value4 = int.Parse(second);
                }
                else if (1 <= voltageValue && voltageValue < 10)
                {
                    value = value * 100;
                    string vslueStr = value.ToString();
                    string[] arry = vslueStr.Split('.');

                    string first = arry[0].Substring(0, 1);
                    string second = arry[0].Substring(1, 1);
                    string third = arry[0].Substring(2, 1);
                    //Debug.Log("first:" + first + "   second:" + second+ "    third:" + third);

                    value0 = 0;
                    value1 = 0;
                    value2 = int.Parse(first); ;
                    value3 = int.Parse(second);
                    value4 = int.Parse(third);
                }
                else if (10 <= voltageValue && voltageValue < 100)
                {
                    value = value * 100;
                    string vslueStr = value.ToString();
                    string[] arry = vslueStr.Split('.');

                    string first = arry[0].Substring(0, 1);
                    string second = arry[0].Substring(1, 1);
                    string third = arry[0].Substring(2, 1);
                    string forth = arry[0].Substring(3, 1);
                    //Debug.Log("first:" + first + "   second:" + second+ "    third:" + third+ "  forth:" + forth);

                    value0 = 0;
                    value1 = int.Parse(first);
                    value2 = int.Parse(second); ;
                    value3 = int.Parse(third);
                    value4 = int.Parse(forth);
                }
                else if (100 <= voltageValue && voltageValue < 1000)
                {
                    value = value * 100;
                    string vslueStr = value.ToString();
                    string[] arry = vslueStr.Split('.');

                    string first = arry[0].Substring(0, 1);
                    string second = arry[0].Substring(1, 1);
                    string third = arry[0].Substring(2, 1);
                    string forth = arry[0].Substring(3, 1);
                    string fifth = arry[0].Substring(4, 1);
                    //Debug.Log("first:" + first + "   second:" + second+ "    third:" + third+ "  forth:" + forth);

                    value0 = int.Parse(first);
                    value1 = int.Parse(second);
                    value2 = int.Parse(third);
                    value3 = int.Parse(forth);
                    value4 = int.Parse(fifth);
                }

                for (int i = 0; i < numbersUV.Count; i++)
                {
                    numbersUV[i].GetPropertyBlock(mpb);
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
                        case 4:
                            {
                                mpb.SetTexture("_MainTex", numberTextures[value4]);
                            }
                            break;
                        default:
                            break;
                    }
                    numbersUV[i].SetPropertyBlock(mpb);
                }
            }
            else
            {
                for (int i = 0; i < numbersUV.Count; i++)
                {
                    numbersUV[i].GetPropertyBlock(mpb);
                    mpb.SetTexture("_MainTex", numberTextures[0]);
                    numbersUV[i].SetPropertyBlock(mpb);
                }
            }
        }
    }
}