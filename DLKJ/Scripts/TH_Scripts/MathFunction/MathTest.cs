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
        private void Awake()
        {
            mathInitValue = new InitValue();
            MathTool.Reset();
        }
        private void Start() => MathTool.δ = 1;

        private void Update() => OnUpdate();

        private void LateUpdate()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    MathTool.FixedCorrect1Calculate();
            //}
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
            Debug.Log(U);
        }

        private InstrumentAction instrumentActionPinXuan;//频选放大器
        private InstrumentButton tempInstrumentBtn;//三厘米测量线
        private InstrumentButton keBianDuanLuQiBtn;//可变断路器
        private InstrumentButton instrumentPiPeiLuoDingD;//匹配螺钉D
        private InstrumentButton instrumentPiPeiLuoDingL;//匹配螺钉L
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