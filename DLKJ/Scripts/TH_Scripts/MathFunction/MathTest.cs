using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DLKJ.InstrumentAction;

namespace DLKJ
{
    public class MathTest : MonoBehaviour
    {
        public InitValue mathInitValue;

        public bool isOpen { get; private set; }
        public bool isFirstClick { get; private set; }
        private static MathTest instance;
        public static MathTest Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<MathTest>();
                return instance;
            }
        }

        GUIStyle gUIStyle = new GUIStyle();
        private void Awake()
        {
            gUIStyle.fontSize = 32;

            mathInitValue = new InitValue();
            MathTool.Reset();
        }
        private void Start() => MathTool.δ = 1;

        private void Update() => OnUpdate();
#if UNITY_EDITOR
        private void LateUpdate()
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //MathTool.FixedCorrect1Calculate();
                MathTool.FixedCorrect2FirstGroupCalculate();
            }

        }
#endif

        //private void OnGUI()
        //{
        //    if (UIManager.GetInstance().showUIData)
        //    {
        //        switch (SceneManager.GetInstance().currentLab.labName)
        //        {
        //            case SceneManager.FIRST_EXPERIMENT_NAME:
        //                {
        //                    UIManager.GetInstance().UIshowDatas1.gameObject.SetActive(true);
        //                    UIManager.GetInstance().UIShowDatas.gameObject.SetActive(false);
        //                    UIManager.GetInstance().UIshowDatas3.gameObject.SetActive(false);
        //                    UIFirst();
        //                }
        //                break;
        //            case SceneManager.SECOND_EXPERIMENT_NAME:
        //                {
        //                    UIManager.GetInstance().UIshowDatas1.gameObject.SetActive(false);
        //                    UIManager.GetInstance().UIShowDatas.gameObject.SetActive(true);
        //                    UIManager.GetInstance().UIshowDatas3.gameObject.SetActive(false);
        //                    UISecond();
        //                }
        //                break;
        //            case SceneManager.THIRD_EXPERIMENT_NAME:
        //                {
        //                    UIManager.GetInstance().UIshowDatas1.gameObject.SetActive(false);
        //                    UIManager.GetInstance().UIShowDatas.gameObject.SetActive(false);
        //                    UIManager.GetInstance().UIshowDatas3.gameObject.SetActive(true);
        //                    UIThird();
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        void UIFirst()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("等效截面：");
            GUILayout.Label(MathTool.report1CorrectAnswer.EquivalentSectionPosition.ToString());
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("等效截面：", MathTool.report1CorrectAnswer.EquivalentSectionPosition);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("输入端波长：", MathTool.report1CorrectAnswer.InputWavelength);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("第一波节点：", MathTool.report1CorrectAnswer.VariableShortCircuitFirstPos[0]);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("第二波节点：", MathTool.report1CorrectAnswer.VariableShortCircuitSecondPos[0]);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("短路器波长：", MathTool.report1CorrectAnswer.VariableWavelengthInShortCircuit);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("开路负载：", MathTool.report1CorrectAnswer.OpenLoadPosition[0]);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("波节点短路：", MathTool.report1CorrectAnswer.WaveNodePosShortCircuit);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("波节点终端：", MathTool.report1CorrectAnswer.WaveNodePosShortTerminal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("波节点匹配：", MathTool.report1CorrectAnswer.WaveNodePosShortMatching);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("相角短路：", MathTool.report1CorrectAnswer.PhaseAngleCircuit);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("相角终端：", MathTool.report1CorrectAnswer.PhaseAngleTerminal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("相角匹配：", MathTool.report1CorrectAnswer.PhaseAngleMatching);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("驻波比短路：", MathTool.report1CorrectAnswer.StandingWaveRatioCircuit);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("驻波比终端：", MathTool.report1CorrectAnswer.StandingWaveRatioTerminal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("驻波比匹配：", MathTool.report1CorrectAnswer.StandingWaveRatioMatching);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("1SReal：", MathTool.report1CorrectAnswer.ReflectionCoefficientΓ1SReal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("1SImaginary：", MathTool.report1CorrectAnswer.ReflectionCoefficientΓ1SImaginary);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("10Real：", MathTool.report1CorrectAnswer.ReflectionCoefficientΓ10Real);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("10Imaginary：", MathTool.report1CorrectAnswer.ReflectionCoefficientΓ10Imaginary);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("1LReal：", MathTool.report1CorrectAnswer.ReflectionCoefficientΓ1LReal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("1LImaginary：", MathTool.report1CorrectAnswer.ReflectionCoefficientΓ1LImaginary);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S11Real：", MathTool.report1CorrectAnswer.inputS11Real);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S11Imaginary：", MathTool.report1CorrectAnswer.inputS11Imaginary);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S21Real：", MathTool.report1CorrectAnswer.inputS12S21Real);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S21Imaginary：", MathTool.report1CorrectAnswer.inputS12S21Imaginary);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S22Real：", MathTool.report1CorrectAnswer.inputS22Real);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S22Imaginary：", MathTool.report1CorrectAnswer.inputS22Imaginary);
        }

        void UISecond()
        {
            UIManager.GetInstance().UIShowDatas.UpdateDatas("电阻：", MathTool.R);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("电抗：", MathTool.X);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("满足条件的结果：", MathTool.verify);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("等效截面：", MathTool.report2CorrectAnswer.EquivalentSectionPositionFirst);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("最小电压：", MathTool.report2CorrectAnswer.MinimumVoltage);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("最大电压：", MathTool.report2CorrectAnswer.MaximumVoltage);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("驻波比：", MathTool.report2CorrectAnswer.SWRFirst);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("第一波节点：", MathTool.report2CorrectAnswer.WaveNodePositionFirst);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("归一电抗：", MathTool.report2CorrectAnswer.NormalizedLoadImpedanceFirstImaginary);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("归一电阻：", MathTool.report2CorrectAnswer.NormalizedLoadImpedanceFirstReal);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("负载电阻：", MathTool.report2CorrectAnswer.LoadImpedanceFirstReal);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("负载电抗：", MathTool.report2CorrectAnswer.LoadImpedanceFirstImaginary);

            if (MathTool.report2CorrectAnswer.ScrewPositionFirst != null && MathTool.report2CorrectAnswer.ScrewPositionFirst.Count > 0) UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配螺钉位置：", MathTool.report2CorrectAnswer.ScrewPositionFirst[0]);
            if (MathTool.report2CorrectAnswer.ScrewDepthFirst != null && MathTool.report2CorrectAnswer.ScrewDepthFirst.Count > 0) UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配螺钉深度：", MathTool.report2CorrectAnswer.ScrewDepthFirst[0]);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配最小电压：", MathTool.report2CorrectAnswer.MinimumVoltageAfterMatchingFirst);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配最大电压：", MathTool.report2CorrectAnswer.MaximumVoltageAfterMatchingFirst);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配驻波比：", MathTool.report2CorrectAnswer.SWRAfterMatchingFirst);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("波导波长：", MathTool.report2CorrectAnswer.WaveguideWavelengthFirst);
        }

        void UIThird()
        {
            UIManager.GetInstance().UIshowDatas3.UpdateDatas("一端口电压：", MathTool.report3CorrectAnswer.OnePortVoltage);
            UIManager.GetInstance().UIshowDatas3.UpdateDatas("三端口电压：", MathTool.report3CorrectAnswer.ThreePortVoltage);
            UIManager.GetInstance().UIshowDatas3.UpdateDatas("耦合度C：", MathTool.report3CorrectAnswer.CouplingFactor);
        }


        /// <summary>
        /// 公示数据初始化
        /// </summary>
        public void FormulaInit()
        {
            if (isFirstClick == false)
            {
                GetDevice();
                SceneManager.GetInstance().GetInstrumentButton("选频放大器", "RotaryBtnVoltage").RemoveListener();
                SceneManager.GetInstance().GetInstrumentButton("选频放大器", "RotaryBtnVoltage").SetInteractiveState(false);
                InitA();
                isFirstClick = true;
            }
        }

        public void IsOpen(bool state)
        {
            isOpen = state;
        }
        /// <summary>
        /// 初始化电压
        /// </summary>
        private void InitA()
        {
            InstrumentActionPinXuan.pointer.SetAngle(MathTool.A);
            Item item = SceneManager.GetInstance().GetItemByName("选频放大器");
            InstrumentAction instrumentAction = item.GetComponent<InstrumentAction>();
            instrumentAction.transform.Find("电压Text").GetComponent<TextMesh>().text = MathTool.A.ToString("#0.00");
        }

        private void OnUpdate()
        {
            if (isOpen == false) return;
            if (!SceneManager.GetInstance().VerifyBasicLink())
            {
                instrumentActionPinXuan.pointer.SetAngle(0);
                instrumentActionPinXuan.pointer.PointerRotate();
                return;
            }
            double U = -10000;

            switch (SceneManager.GetInstance().currentLab.labName)
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                    switch (SceneManager.GetInstance().currentLab.currentStepIndex)
                    {
                        case 2:
                        case 3:
                        case 4:
                            U = MathTool.SLMCLXZDDLB(MathUtility.GetDistance(tempInstrumentBtn));

                            break;
                        case 5:
                            Debug.Log("接二端口和短路板U:");
                            U = MathTool.EDKTODLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        case 6:
                            Debug.Log("接可变断路器U:");
                            //   U = MathTool.ErDuanKouKeBianDuanLuQi(MathUtility.GetDistance(keBianDuanLuQiBtn), MathUtility.GetDistance(tempInstrumentBtn));
                            U = MathTool.ErDuanKouKeBianDuanLuQiNew(MathUtility.GetDistance(keBianDuanLuQiBtn), MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        case 7:
                            U = MathTool.EDKPPFZ(MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        default:
                            break;
                    }
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    switch (SceneManager.GetInstance().currentLab.currentStepIndex)
                    {
                        case 2:
                        case 3:
                        case 4:
                            U = MathTool.SLMCLXZDDLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 5:
                            U = MathTool.FZZKCL_First(MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 6:
                            U = MathTool.FZKZPP(MathUtility.GetDistance(instrumentPiPeiLuoDingL), MathUtility.GetDistance(instrumentPiPeiLuoDingD), MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 7:
                            U = MathTool.SLMCLXZDDLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 8:
                            U = MathTool.FZZKCL_First(MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 9:
                            U = MathTool.FZKZPP(MathUtility.GetDistance(instrumentPiPeiLuoDingL), MathUtility.GetDistance(instrumentPiPeiLuoDingD), MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        default:
                            break;
                    }
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:

                    switch (SceneManager.GetInstance().currentLab.currentStepIndex)
                    {
                        case 1:
                        case 2:
                        case 3:
                            U = MathTool.OnePortVoltage();
                            break;
                        case 4:
                            U = MathTool.CouplingFactorObserved();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            if (double.IsNaN(U))
            {
                instrumentActionPinXuan.pointer.SetAngle(0);
                return;
            }
            instrumentActionPinXuan.pointer.SetAngle(float.Parse(U.ToString()));
            instrumentActionPinXuan.pointer.PointerRotate();
            //Debug.Log(U);
        }

        private InstrumentAction instrumentActionPinXuan;//频选放大器
        private InstrumentButton tempInstrumentBtn;//三厘米测量线
        private InstrumentButton keBianDuanLuQiBtn;//可变断路器
        private InstrumentButton instrumentPiPeiLuoDingD;//匹配螺钉D
        private InstrumentButton instrumentPiPeiLuoDingL;//匹配螺钉L


        public InstrumentAction InstrumentActionPinXuan
        {
            get
            {
                if (instrumentActionPinXuan == null)
                {
                    Item itemPinXuan = SceneManager.GetInstance().GetItemByName("选频放大器");
                    instrumentActionPinXuan = itemPinXuan.GetComponent<InstrumentAction>();
                }
                return instrumentActionPinXuan;
            }
        }
        public void GetDevice()
        {
            if (tempInstrumentBtn == null)
            {
                tempInstrumentBtn = SceneManager.GetInstance().GetInstrumentButton("三厘米测量线", "FrequencySelectKnob");
            }
            if (instrumentActionPinXuan == null)
            {
                Item itemPinXuan = SceneManager.GetInstance().GetItemByName("选频放大器");
                instrumentActionPinXuan = itemPinXuan.GetComponent<InstrumentAction>();
            }
            if (keBianDuanLuQiBtn == null)
            {
                keBianDuanLuQiBtn = SceneManager.GetInstance().GetInstrumentButton("可变短路器", "kebianduanluqi4");
            }

            if (instrumentPiPeiLuoDingL == null)
            {
                instrumentPiPeiLuoDingL = SceneManager.GetInstance().GetInstrumentButton("匹配螺钉", "PiPeiLuoDingBtn");
            }
            if (instrumentPiPeiLuoDingD == null)
            {
                instrumentPiPeiLuoDingD = SceneManager.GetInstance().GetInstrumentButton("匹配螺钉", "PPLDGear");
            }
        }

        public bool CheckValueIsInit()
        {
            if (SceneManager.GetInstance().currentLab.labName == SceneManager.THIRD_EXPERIMENT_NAME)
            {
                return mathInitValue.initF == true && mathInitValue.initA;
            }
            else
            {
                return mathInitValue.initF == true && mathInitValue.initδ == true && mathInitValue.initA;
            }
        }
    }
}