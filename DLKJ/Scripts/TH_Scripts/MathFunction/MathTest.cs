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
        /// ��ʾ���ݳ�ʼ��
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
                case "���˿�΢�������������":
                    switch (SceneManager.GetInstance().currentLab.currentStepIndex)
                    {
                        case 1:

                            Item item = SceneManager.GetInstance().GetItemByName("�����ײ�����");
                            InstrumentAction instrumentAction = item.GetComponent<InstrumentAction>();
                            InstrumentButton tempInstrumentBtn = instrumentAction.instrumentButton.Find(x => x.instrumentButton.name == "FrequencySelectKnob");
                            Item itemPinXuan = SceneManager.GetInstance().GetItemByName("Ƶѡ�Ŵ���");
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