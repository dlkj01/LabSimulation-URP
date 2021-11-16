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
        private void Start() => MathTool.�� = 1;

        private void Update() => OnUpdate();

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
        /// ��ʼ����ѹ
        /// </summary>
        private void InitA()
        {
            instrumentActionPinXuan.pointer.SetAngle(MathTool.A);
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
                            U = MathTool.EDKTODLB(MathUtility.GetDistance(tempInstrumentBtn));
                            break;
                        case 6:
                            U = MathTool.ErDuanKouKeBianDuanLuQi(MathUtility.GetDistance(keBianDuanLuQiBtn), MathUtility.GetDistance(tempInstrumentBtn));
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