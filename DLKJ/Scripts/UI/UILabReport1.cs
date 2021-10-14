using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DLKJ
{
    public class UILabReport1 : UILabReportBase
    {
        [SerializeField] InputField nameInputField;
        [SerializeField] InputField classInputField;
        [SerializeField] InputField timeInputField;
        [SerializeField] InputField idInputField;
        [SerializeField] InputField teacherInputField;

        public void OnSureCallBack()
        {
            if (nameInputField.text.Length > 0)
            {

            }
            else
            {
                EventManager.OnTips(TipsType.Toast, "请输入姓名");
            }
        }
        [SerializeField] InputField SourceFrequency;//信号源频率
        [SerializeField] InputField SourceVoltage;//信号源电压设置
        [SerializeField] InputField Attenuator;//衰减器设置
        [SerializeField] InputField EquivalentSectionPosition;//等效截面位置
        [SerializeField] InputField InputWavelength;//输入端波长
        [SerializeField] InputField VariableShortCircuitFirstPos;//可变短路器第一波节点位置
        [SerializeField] InputField VariableShortCircuitSecondPos;//可变短路器第二波节点位置
        [SerializeField] InputField VariableWavelengthInShortCircuit;//可变短路器中波长 
        [SerializeField] InputField OpenLoadPosition;//开路负载位置
        [SerializeField] InputField WaveNodePosShortCircuit;//波节点位置短路
        [SerializeField] InputField WaveNodePosShortTerminal;//波节点位置终端
        [SerializeField] InputField WaveNodePosShortMatching;//波节点位置匹配
        [SerializeField] InputField PhaseAngleCircuit;//相角短路
        [SerializeField] InputField PhaseAngleTerminal;//相角终端
        [SerializeField] InputField PhaseAngleMatching;//相角匹配
        [SerializeField] InputField StandingWaveRatioCircuit;//驻波比短路
        [SerializeField] InputField StandingWaveRatioTerminal;//驻波比终端
        [SerializeField] InputField StandingWaveRatioMatching;//驻波比匹配
        [SerializeField] InputField inputΓ1S;
        [SerializeField] InputField inputΓ10;
        [SerializeField] InputField inputΓ1L;
        [SerializeField] InputField ReflectionCoefficientΓ1S;//反射系数短路
        [SerializeField] InputField ReflectionCoefficientΓ10;//反射系数终端
        [SerializeField] InputField ReflectionCoefficientΓ1L;//反射系数匹配
        [SerializeField] InputField inputS11;
        [SerializeField] InputField inputS12S21;
        [SerializeField] InputField inputS22;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SetVisibale(false);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                SetVisibale(true);
            }
        }
    }
}