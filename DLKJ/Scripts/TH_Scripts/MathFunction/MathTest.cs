using System.Collections;
using System.Collections.Generic;
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
            isInit = true;
        }
        public void Active(bool state)
        {
            isInit = state;
        }
        private void Start()
        {

            //      MathTool.Init();
        }
        private void Update()
        {
            OnUpdate();
        }



        private void OnUpdate()
        {
            if (isInit == false) return;
            switch (SceneManager.GetInstance().currentLab.labName)
            {
                case "二端口微波网络参量测量":
                    switch (SceneManager.GetInstance().currentLab.currentStepIndex)
                    {
                        case 1:

                            Item item = SceneManager.GetInstance().GetItemByName("三厘米测量线");
                            InstrumentAction instrumentAction = item.GetComponent<InstrumentAction>();
                            InstrumentButton tempInstrumentBtn = instrumentAction.instrumentButton.Find(x => x.instrumentButton.name == "FrequencySelectKnob");
                            Item itemPinXuan = SceneManager.GetInstance().GetItemByName("频选放大器");
                            InstrumentAction instrumentActionPinXuan = itemPinXuan.GetComponent<InstrumentAction>();
                            double U = MathTool.SLMCLXZDDLB(MathUtility.GetSanLiMiCeLiangXianDistance(tempInstrumentBtn));
                            instrumentActionPinXuan.pointer.SetAngle(float.Parse(U.ToString()));
                            Debug.Log(U);
                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

        }
    }
}