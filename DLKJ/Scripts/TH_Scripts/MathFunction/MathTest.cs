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
        /// 公示数据初始化
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
                MathTool.FixedCorrect2FirstGroupCalculate();
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
                case "二端口微波网络参量测量":
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
                case "负载阻抗测量":
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
                case " 负载阻抗匹配和定向耦合器特性的测量":

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
                Debug.LogError("计算的结果无效");
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
        private void ShiYan1()
        {
            if (tempInstrumentBtn == null)
            {
                tempInstrumentBtn = GetInstrumentButton("三厘米测量线", "FrequencySelectKnob");
            }
            if (instrumentActionPinXuan == null)
            {
                Item itemPinXuan = SceneManager.GetInstance().GetItemByName("频选放大器");
                instrumentActionPinXuan = itemPinXuan.GetComponent<InstrumentAction>();
            }
            if (keBianDuanLuQiBtn == null)
            {
                keBianDuanLuQiBtn = GetInstrumentButton("可变短路器", "kebianduanluqi4");
            }

            if (instrumentPiPeiLuoDingL == null)
            {
                instrumentPiPeiLuoDingL = GetInstrumentButton("匹配螺钉", "PiPeiLuoDingBtn");
            }
            if (instrumentPiPeiLuoDingD == null)
            {
                instrumentPiPeiLuoDingD = GetInstrumentButton("匹配螺钉", "PPLDGear");
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