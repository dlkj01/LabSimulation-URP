using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
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

        private void OnGUI()
        {
            //GUILayout.BeginHorizontal();
            //GUILayout.Label("<color=#0300ff>R：" + MathTool.R + "        X:" + MathTool.X + "        Verify:" + MathTool.verify+ "</color>  ", gUIStyle, new GUILayoutOption[] { GUILayout.Width(500) });
            //GUILayout.EndHorizontal();
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

            if(MathTool.report2CorrectAnswer.ScrewPositionFirst != null) UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配螺钉位置：", MathTool.report2CorrectAnswer.ScrewPositionFirst[0]);
           if(MathTool.report2CorrectAnswer.ScrewDepthFirst != null) UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配螺钉深度：", MathTool.report2CorrectAnswer.ScrewDepthFirst[0]);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配最小电压：", MathTool.report2CorrectAnswer.MinimumVoltageAfterMatchingFirst);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配最大电压：", MathTool.report2CorrectAnswer.MaximumVoltageAfterMatchingSecond);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("匹配驻波比：", MathTool.report2CorrectAnswer.SWRAfterMatchingSecond);
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
                MathTool.Init();
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
            instrumentActionPinXuan.pointer.SetAngle(MathTool.A);
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
                            U = MathTool.EDKTODLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        case 6:
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
        private void GetDevice()
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