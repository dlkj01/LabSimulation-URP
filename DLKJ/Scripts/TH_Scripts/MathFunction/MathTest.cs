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
        public bool isInit { get; private set; }
        public FormulaData formulaData;
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

        /// <summary>
        /// ��ʾ���ݳ�ʼ��
        /// </summary>
        public void FormulaInit()
        {
            MathTool.Init();
            ShiYan1();
            Active(true);
        }
        public void Active(bool state)
        {
            isInit = state;
        }
        private void Start()
        {
            MathTool.Init();
        }

        private void Update()
        {
            OnUpdate();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //MathTool.GetDT(MathTool.SLMCL_Start_Value, 0);
                //MathTool.GetFirstMinBoundKBDLQ(0, 1);
                //MathTool.GetFirstMinBoundKBDLQ(0, 2);
                MathTool.FixedCorrect2Calculate();
            }
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
                case "���˿�΢�������������":
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
                case "�����迹����":
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
                        default:
                            break;
                    }
                    break;
                case " �����迹ƥ��Ͷ�����������ԵĲ���":

                    switch (SceneManager.GetInstance().currentLab.currentStepIndex)
                    {
                        case 1:
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
                Debug.LogError("����Ľ����Ч");
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
        private void ShiYan1()
        {
            if (tempInstrumentBtn == null)
            {
                tempInstrumentBtn = GetInstrumentButton("�����ײ�����", "FrequencySelectKnob");
            }
            if (instrumentActionPinXuan == null)
            {
                Item itemPinXuan = SceneManager.GetInstance().GetItemByName("Ƶѡ�Ŵ���");
                instrumentActionPinXuan = itemPinXuan.GetComponent<InstrumentAction>();
            }
            if (keBianDuanLuQiBtn == null)
            {
                keBianDuanLuQiBtn = GetInstrumentButton("�ɱ��·��", "kebianduanluqi4");
            }

            if (instrumentPiPeiLuoDingD == null)
            {
                instrumentPiPeiLuoDingL = GetInstrumentButton("ƥ���ݶ�", "PiPeiLuoDingBtn");
            }
            if (instrumentPiPeiLuoDingL == null)
            {
                instrumentPiPeiLuoDingL = GetInstrumentButton("ƥ���ݶ�", "PPLDGear");
            }

        }

        private InstrumentButton GetInstrumentButton(string deviceName, string buttonName)
        {
            Item item = SceneManager.GetInstance().GetItemByName(deviceName);
            if (item == null)
                return null;
            InstrumentAction instrumentAction = item.GetComponent<InstrumentAction>();
            if (instrumentAction == null)
                return null;
            InstrumentButton button = instrumentAction.instrumentButton.Find(x => x.instrumentButton.name == buttonName);
            return button;
        }
    }
}