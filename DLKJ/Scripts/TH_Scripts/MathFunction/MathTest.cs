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
        private void Start() => MathTool.�� = 1;

        private void Update() => OnUpdate();
#if UNITY_EDITOR
        //private void LateUpdate()
        //{

        //    if (Input.GetKeyDown(KeyCode.Alpha1))
        //    {
        //        //MathTool.FixedCorrect1Calculate();
        //        MathTool.FixedCorrect2FirstGroupCalculate();
        //    }

        //}
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
            GUILayout.Label("��Ч���棺");
            GUILayout.Label(MathTool.report1CorrectAnswer.EquivalentSectionPosition.ToString());
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("��Ч���棺", MathTool.report1CorrectAnswer.EquivalentSectionPosition);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("����˲�����", MathTool.report1CorrectAnswer.InputWavelength);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("��һ���ڵ㣺", MathTool.report1CorrectAnswer.VariableShortCircuitFirstPos[0]);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("�ڶ����ڵ㣺", MathTool.report1CorrectAnswer.VariableShortCircuitSecondPos[0]);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("��·��������", MathTool.report1CorrectAnswer.VariableWavelengthInShortCircuit);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("��·���أ�", MathTool.report1CorrectAnswer.OpenLoadPosition[0]);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("���ڵ��·��", MathTool.report1CorrectAnswer.WaveNodePosShortCircuit);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("���ڵ��նˣ�", MathTool.report1CorrectAnswer.WaveNodePosShortTerminal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("���ڵ�ƥ�䣺", MathTool.report1CorrectAnswer.WaveNodePosShortMatching);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("��Ƕ�·��", MathTool.report1CorrectAnswer.PhaseAngleCircuit);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("����նˣ�", MathTool.report1CorrectAnswer.PhaseAngleTerminal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("���ƥ�䣺", MathTool.report1CorrectAnswer.PhaseAngleMatching);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("פ���ȶ�·��", MathTool.report1CorrectAnswer.StandingWaveRatioCircuit);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("פ�����նˣ�", MathTool.report1CorrectAnswer.StandingWaveRatioTerminal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("פ����ƥ�䣺", MathTool.report1CorrectAnswer.StandingWaveRatioMatching);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("1SReal��", MathTool.report1CorrectAnswer.ReflectionCoefficient��1SReal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("1SImaginary��", MathTool.report1CorrectAnswer.ReflectionCoefficient��1SImaginary);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("10Real��", MathTool.report1CorrectAnswer.ReflectionCoefficient��10Real);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("10Imaginary��", MathTool.report1CorrectAnswer.ReflectionCoefficient��10Imaginary);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("1LReal��", MathTool.report1CorrectAnswer.ReflectionCoefficient��1LReal);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("1LImaginary��", MathTool.report1CorrectAnswer.ReflectionCoefficient��1LImaginary);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S11Real��", MathTool.report1CorrectAnswer.inputS11Real);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S11Imaginary��", MathTool.report1CorrectAnswer.inputS11Imaginary);

            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S21Real��", MathTool.report1CorrectAnswer.inputS12S21Real);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S21Imaginary��", MathTool.report1CorrectAnswer.inputS12S21Imaginary);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S22Real��", MathTool.report1CorrectAnswer.inputS22Real);
            //UIManager.GetInstance().UIshowDatas1.UpdateDatas("S22Imaginary��", MathTool.report1CorrectAnswer.inputS22Imaginary);
        }

        void UISecond()
        {
            UIManager.GetInstance().UIShowDatas.UpdateDatas("���裺", MathTool.R);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("�翹��", MathTool.X);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("���������Ľ����", MathTool.verify);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("��Ч���棺", MathTool.report2CorrectAnswer.EquivalentSectionPositionFirst);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("��С��ѹ��", MathTool.report2CorrectAnswer.MinimumVoltage);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("����ѹ��", MathTool.report2CorrectAnswer.MaximumVoltage);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("פ���ȣ�", MathTool.report2CorrectAnswer.SWRFirst);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("��һ���ڵ㣺", MathTool.report2CorrectAnswer.WaveNodePositionFirst);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("��һ�翹��", MathTool.report2CorrectAnswer.NormalizedLoadImpedanceFirstImaginary);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("��һ���裺", MathTool.report2CorrectAnswer.NormalizedLoadImpedanceFirstReal);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("���ص��裺", MathTool.report2CorrectAnswer.LoadImpedanceFirstReal);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("���ص翹��", MathTool.report2CorrectAnswer.LoadImpedanceFirstImaginary);

            if (MathTool.report2CorrectAnswer.ScrewPositionFirst != null && MathTool.report2CorrectAnswer.ScrewPositionFirst.Count > 0) UIManager.GetInstance().UIShowDatas.UpdateDatas("ƥ���ݶ�λ�ã�", MathTool.report2CorrectAnswer.ScrewPositionFirst[0]);
            if (MathTool.report2CorrectAnswer.ScrewDepthFirst != null && MathTool.report2CorrectAnswer.ScrewDepthFirst.Count > 0) UIManager.GetInstance().UIShowDatas.UpdateDatas("ƥ���ݶ���ȣ�", MathTool.report2CorrectAnswer.ScrewDepthFirst[0]);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("ƥ����С��ѹ��", MathTool.report2CorrectAnswer.MinimumVoltageAfterMatchingFirst);

            UIManager.GetInstance().UIShowDatas.UpdateDatas("ƥ������ѹ��", MathTool.report2CorrectAnswer.MaximumVoltageAfterMatchingFirst);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("ƥ��פ���ȣ�", MathTool.report2CorrectAnswer.SWRAfterMatchingFirst);
            UIManager.GetInstance().UIShowDatas.UpdateDatas("����������", MathTool.report2CorrectAnswer.WaveguideWavelengthFirst);
        }

        void UIThird()
        {
            UIManager.GetInstance().UIshowDatas3.UpdateDatas("һ�˿ڵ�ѹ��", MathTool.report3CorrectAnswer.OnePortVoltage);
            UIManager.GetInstance().UIshowDatas3.UpdateDatas("���˿ڵ�ѹ��", MathTool.report3CorrectAnswer.ThreePortVoltage);
            UIManager.GetInstance().UIshowDatas3.UpdateDatas("��϶�C��", MathTool.report3CorrectAnswer.CouplingFactor);
        }


        /// <summary>
        /// ��ʾ���ݳ�ʼ��
        /// </summary>
        public void FormulaInit()
        {
            if (isFirstClick == false)
            {
                GetDevice();
                SceneManager.GetInstance().GetInstrumentButton("ѡƵ�Ŵ���", "RotaryBtnVoltage").RemoveListener();
                SceneManager.GetInstance().GetInstrumentButton("ѡƵ�Ŵ���", "RotaryBtnVoltage").SetInteractiveState(false);
                InitA();
                isFirstClick = true;
            }
        }

        public void IsOpen(bool state)
        {
            isOpen = state;
        }
        /// <summary>
        /// ��ʼ����ѹ
        /// </summary>
        private void InitA()
        {
            InstrumentActionPinXuan.pointer.SetAngle(MathTool.A);
            Item item = SceneManager.GetInstance().GetItemByName("ѡƵ�Ŵ���");
            InstrumentAction instrumentAction = item.GetComponent<InstrumentAction>();
            instrumentAction.transform.Find("��ѹText").GetComponent<TextMesh>().text = MathTool.A.ToString("#0.00");
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
                            //Debug.Log("�Ӷ��˿ںͶ�·��U:");
                            U = MathTool.EDKTODLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        case 6:
                            //Debug.Log("�ӿɱ��·��U:");
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

        private InstrumentAction instrumentActionPinXuan;//Ƶѡ�Ŵ���
        private InstrumentButton tempInstrumentBtn;//�����ײ�����
        private InstrumentButton keBianDuanLuQiBtn;//�ɱ��·��
        private InstrumentButton instrumentPiPeiLuoDingD;//ƥ���ݶ�D
        private InstrumentButton instrumentPiPeiLuoDingL;//ƥ���ݶ�L


        public InstrumentAction InstrumentActionPinXuan
        {
            get
            {
                if (instrumentActionPinXuan == null)
                {
                    Item itemPinXuan = SceneManager.GetInstance().GetItemByName("ѡƵ�Ŵ���");
                    instrumentActionPinXuan = itemPinXuan.GetComponent<InstrumentAction>();
                }
                return instrumentActionPinXuan;
            }
        }
        public void GetDevice()
        {
            if (tempInstrumentBtn == null)
            {
                tempInstrumentBtn = SceneManager.GetInstance().GetInstrumentButton("�����ײ�����", "FrequencySelectKnob");
            }
            if (instrumentActionPinXuan == null)
            {
                Item itemPinXuan = SceneManager.GetInstance().GetItemByName("ѡƵ�Ŵ���");
                instrumentActionPinXuan = itemPinXuan.GetComponent<InstrumentAction>();
            }
            if (keBianDuanLuQiBtn == null)
            {
                keBianDuanLuQiBtn = SceneManager.GetInstance().GetInstrumentButton("�ɱ��·��", "kebianduanluqi4");
            }

            if (instrumentPiPeiLuoDingL == null)
            {
                instrumentPiPeiLuoDingL = SceneManager.GetInstance().GetInstrumentButton("ƥ���ݶ�", "PiPeiLuoDingBtn");
            }
            if (instrumentPiPeiLuoDingD == null)
            {
                instrumentPiPeiLuoDingD = SceneManager.GetInstance().GetInstrumentButton("ƥ���ݶ�", "PPLDGear");
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
                return mathInitValue.initF == true && mathInitValue.init�� == true && mathInitValue.initA;
            }
        }
    }
}