using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static DLKJ.InstrumentAction;

namespace DLKJ
{
    public class MathTest : MonoBehaviour
    {
        public InitValue mathInitValue;
        private void Awake()
        {
            mathInitValue = new InitValue();
        }
        public bool isInit { get; private set; }
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

        [HideInInspector] public float randomAngle;

        /// <summary>
        /// ��ʾ���ݳ�ʼ��
        /// </summary>
        public void FormulaInit()
        {
            MathTool.Init();
            GetDevice();
            Active(true);
            //����ѹ�Զ�����һ�����ֵ
            InitA();
            //�Ƴ�Ƶѡ�Ŵ�����ť�ļ���
            SceneManager.GetInstance().GetInstrumentButton("ѡƵ�Ŵ���", "RotaryBtnVoltage").RemoveListener();
            SceneManager.GetInstance().GetInstrumentButton("ѡƵ�Ŵ���", "RotaryBtnVoltage").SetInteractiveState(false);
        }
        public void Active(bool state)
        {
            isInit = state;
        }
        /// <summary>
        /// ��ʼ����ѹ
        /// </summary>
        private void InitA()
        {
            instrumentActionPinXuan.pointer.SetAngle(MathTool.A);
            Item item = SceneManager.GetInstance().GetItemByName("ѡƵ�Ŵ���");
            InstrumentAction instrumentAction = item.GetComponent<InstrumentAction>();
            instrumentAction.transform.Find("��ѹText").GetComponent<TextMesh>().text = MathTool.A.ToString("#0.00");
        }
        private void Start()
        {
            //MathTool.Init();
            MathTool.A =861.72f/* UnityEngine.Random.Range(2f, 2000f)*/;
            Debug.Log(MathTool.A);
        }

        private void Update()
        {
            OnUpdate();
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    MathTool.FixedCorrect1Calculate();
            //}
        }


        private void OnUpdate()
        {
            if (isInit == false) return;
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
                            U = MathTool.SLMCLXZDDLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        case 3:
                            U = MathTool.EDKTODLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        case 4:
                            U = MathTool.ErDuanKouKeBianDuanLuQi(MathUtility.GetDistance(keBianDuanLuQiBtn), MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        case 5:
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
                            U = MathTool.SLMCLXZDDLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 3:
                            U = MathTool.FZZKCL_First(MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 4:
                            U = MathTool.FZKZPP(MathUtility.GetDistance(instrumentPiPeiLuoDingL), MathUtility.GetDistance(instrumentPiPeiLuoDingD), MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 5:
                            U = MathTool.SLMCLXZDDLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 6:
                            U = MathTool.FZZKCL_First(MathUtility.GetDistance(tempInstrumentBtn));
                            break;

                        case 7:
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
                            U = MathTool.OnePortVoltage();
                            break;
                        case 2:
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

        private InstrumentAction instrumentActionPinXuan;//Ƶѡ�Ŵ���
        private InstrumentButton tempInstrumentBtn;//�����ײ�����
        private InstrumentButton keBianDuanLuQiBtn;//�ɱ��·��
        private InstrumentButton instrumentPiPeiLuoDingD;//ƥ���ݶ�D
        private InstrumentButton instrumentPiPeiLuoDingL;//ƥ���ݶ�L
        private void GetDevice()
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
            if (SceneManager.GetInstance().currentLab.labName == "�����迹ƥ��Ͷ�����������ԵĲ���")
            {
                return mathInitValue.initF == true;
            }
            else
            {
                return mathInitValue.initF == true && mathInitValue.init�� == true;
            }
        }
    }
}